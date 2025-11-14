<template>
  <div class="wishlist">
    <div class="card">
      <div class="card-body">
        <h5 class="card-title mb-4">願望清單</h5>
        
        <div v-if="loading" class="text-center">
          <div class="spinner-border text-primary" role="status">
            <span class="visually-hidden">Loading...</span>
          </div>
        </div>

        <div v-else-if="wishlistItems.length === 0" class="text-center">
          <p>您的願望清單目前是空的</p>
          <router-link to="/products" class="btn btn-primary">
            瀏覽商品
          </router-link>
        </div>

        <div v-else class="wishlist-items">
          <div class="row">
            <div v-for="item in wishlistItems" :key="item.id" class="col-md-4 mb-4">
              <div class="card h-100">
                <img :src="item.image" class="card-img-top" :alt="item.name">
                <div class="card-body">
                  <h5 class="card-title">{{ item.name }}</h5>
                  <p class="card-text text-primary">NT$ {{ item.price }}</p>
                </div>
                <div class="card-footer bg-transparent">
                  <div class="d-flex justify-content-between">
                    <button class="btn btn-primary" @click="addToCart(item)">
                      加入購物車
                    </button>
                    <button class="btn btn-outline-danger" @click="removeFromWishlist(item.id)">
                      <i class="bi bi-heart-fill"></i>
                    </button>
                  </div>
                </div>
              </div>
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

const store = useStore()

// 響應式數據
const loading = ref(false)
const wishlistItems = ref([])

// Computed
const user = computed(() => store.state.auth.user)

// 方法
const loadWishlist = async () => {
  if (!user.value) return
  loading.value = true
  try {
    wishlistItems.value = await store.dispatch('fetchWishlist')
  } catch (error) {
    console.error('Failed to fetch wishlist:', error)
  } finally {
    loading.value = false
  }
}

const addToCart = async (item) => {
  try {
    await store.dispatch('addToCart', item)
    alert('已加入購物車')
  } catch (error) {
    alert('加入購物車失敗')
  }
}

const removeFromWishlist = async (itemId) => {
  try {
    await store.dispatch('removeFromWishlist', itemId)
    wishlistItems.value = wishlistItems.value.filter(item => item.id !== itemId)
  } catch (error) {
    alert('移除失敗')
  }
}

// 初始化
onMounted(() => {
  loadWishlist()
})
</script>

<style scoped>
.card-img-top {
  height: 200px;
  object-fit: cover;
}

.card {
  transition: transform 0.2s;
}

.card:hover {
  transform: translateY(-5px);
}
</style> 