<template>
  <div class="product-detail">
    <div class="container py-4">
      <div v-if="product" class="row">
        <!-- Product Images -->
        <div class="col-md-6">
          <div class="card mb-4">
            <img :src="product.imageUrl" class="card-img-top" :alt="product.name">
          </div>
          <div class="row">
            <div v-for="(image, index) in product.images" :key="index" class="col-3">
              <img :src="image" class="img-thumbnail" :alt="product.name + ' ' + (index + 1)">
            </div>
          </div>
        </div>

        <!-- Product Info -->
        <div class="col-md-6">
          <h1 class="mb-3">{{ product.name }}</h1>
          <p class="text-muted mb-3">商品編號: {{ product.id }}</p>
          <div class="d-flex align-items-center mb-3">
            <h3 class="text-primary mb-0">NT$ {{ product.price }}</h3>
            <span v-if="product.stock > 0" class="badge bg-success ms-3">有現貨</span>
            <span v-else class="badge bg-danger ms-3">缺貨中</span>
          </div>
          <p class="mb-4">{{ product.description }}</p>

          <!-- Quantity Selector -->
          <div class="mb-4">
            <label class="form-label">數量</label>
            <div class="input-group" style="width: 150px;">
              <button class="btn btn-outline-secondary" @click="decreaseQuantity">-</button>
              <input type="number" v-model="quantity" class="form-control text-center" min="1" :max="product.stock">
              <button class="btn btn-outline-secondary" @click="increaseQuantity">+</button>
            </div>
          </div>

          <!-- Action Buttons -->
          <div class="d-grid gap-2">
            <button @click="addToCart" class="btn btn-primary" :disabled="product.stock === 0">
              加入購物車
            </button>
            <button @click="addToWishlist" class="btn btn-outline-primary">
              加入願望清單
            </button>
          </div>

          <!-- Product Details -->
          <div class="mt-4">
            <h5>商品詳情</h5>
            <ul class="list-unstyled">
              <li><strong>產地：</strong>{{ product.origin }}</li>
              <li><strong>保存方式：</strong>{{ product.storage }}</li>
              <li><strong>營養成分：</strong>{{ product.nutrition }}</li>
            </ul>
          </div>
        </div>
      </div>

      <!-- Reviews Section -->
      <div class="row mt-5">
        <div class="col-12">
          <h3>商品評價</h3>
          <div class="card">
            <div class="card-body">
              <!-- Review Form -->
              <div v-if="isAuthenticated" class="mb-4">
                <h5>發表評價</h5>
                <form @submit.prevent="submitReview">
                  <div class="mb-3">
                    <label class="form-label">評分</label>
                    <div class="rating">
                      <i v-for="star in 5" :key="star" 
                         class="bi" 
                         :class="star <= review.rating ? 'bi-star-fill' : 'bi-star'"
                         @click="review.rating = star"></i>
                    </div>
                  </div>
                  <div class="mb-3">
                    <label class="form-label">評價內容</label>
                    <textarea v-model="review.content" class="form-control" rows="3"></textarea>
                  </div>
                  <button type="submit" class="btn btn-primary">提交評價</button>
                </form>
              </div>

              <!-- Reviews List -->
              <div v-if="reviews.length > 0">
                <div v-for="review in reviews" :key="review.id" class="review-item mb-3">
                  <div class="d-flex justify-content-between">
                    <h6>{{ review.userName }}</h6>
                    <small class="text-muted">{{ formatDate(review.createdAt) }}</small>
                  </div>
                  <div class="rating mb-2">
                    <i v-for="star in 5" :key="star" 
                       class="bi" 
                       :class="star <= review.rating ? 'bi-star-fill' : 'bi-star'"></i>
                  </div>
                  <p>{{ review.content }}</p>
                </div>
              </div>
              <p v-else class="text-muted">暫無評價</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useStore } from 'vuex'
import { useRoute } from 'vue-router'

const store = useStore()
const route = useRoute()

// 響應式數據
const quantity = ref(1)
const review = ref({
  rating: 0,
  content: ''
})

// Computed
const product = computed(() => store.state.products.current)
const reviews = computed(() => store.state.products.reviews)
const isAuthenticated = computed(() => store.getters['auth/isAuthenticated'])

// 方法
const increaseQuantity = () => {
  if (quantity.value < product.value.stock) {
    quantity.value++
  }
}

const decreaseQuantity = () => {
  if (quantity.value > 1) {
    quantity.value--
  }
}

const addToCart = async () => {
  await store.dispatch('cart/addToCart', { 
    productId: product.value.id, 
    quantity: quantity.value 
  })
}

const addToWishlist = async () => {
  await store.dispatch('wishlist/addToWishlist', product.value.id)
}

const submitReview = async () => {
  if (review.value.rating > 0 && review.value.content) {
    await store.dispatch('products/submitReview', {
      productId: product.value.id,
      ...review.value
    })
    review.value = { rating: 0, content: '' }
  }
}

const formatDate = (date) => {
  return new Date(date).toLocaleDateString('zh-TW')
}

// 生命週期
onMounted(async () => {
  const productId = route.params.id
  await store.dispatch('products/fetchProduct', productId)
  await store.dispatch('products/fetchReviews', productId)
})
</script>

<style scoped>
.card-img-top {
  height: 400px;
  object-fit: cover;
}

.img-thumbnail {
  height: 100px;
  object-fit: cover;
  cursor: pointer;
}

.rating {
  color: #ffc107;
  font-size: 1.5rem;
}

.rating i {
  cursor: pointer;
}

.review-item {
  border-bottom: 1px solid #eee;
  padding-bottom: 1rem;
}

.review-item:last-child {
  border-bottom: none;
}
</style> 