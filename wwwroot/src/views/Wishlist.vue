<template>
  <div class="wishlist">
    <div class="container py-5">
      <h2 class="mb-4">我的願望清單</h2>

      <div v-if="wishlistItems.length === 0" class="text-center py-5">
        <i class="bi bi-heart display-1 text-muted"></i>
        <h3 class="mt-3">您的願望清單是空的</h3>
        <p class="text-muted mb-4">快去選購您喜歡的商品吧！</p>
        <router-link to="/products" class="btn btn-primary">前往購物</router-link>
      </div>

      <div v-else class="row">
        <div v-for="item in wishlistItems" :key="item.id" class="col-md-4 mb-4">
          <div class="card h-100">
            <img :src="getProductImage(item.categoryId)" class="card-img-top" :alt="item.name">
            <div class="card-body">
              <h5 class="card-title">{{ item.name }}</h5>
              <p class="card-text">{{ item.description }}</p>
              <p class="card-text text-primary">NT$ {{ item.price }}</p>
              <div class="d-flex justify-content-between">
                <router-link :to="'/products/' + item.id" class="btn btn-outline-primary">
                  查看詳情
                </router-link>
                <button @click="addToCart(item)" class="btn btn-primary">
                  加入購物車
                </button>
                <button @click="removeFromWishlist(item)" class="btn btn-outline-danger">
                  <i class="bi bi-heart-fill"></i>
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed, watch } from 'vue'
import { useStore } from 'vuex'

const store = useStore()

// Computed
const wishlistItems = computed(() => store.state.wishlist.items || [])

// 方法
const removeFromWishlist = (item) => {
  store.dispatch('wishlist/removeFromWishlist', item)
}

const addToCart = (product) => {
  store.dispatch('cart/addToCart', product)
}

const getProductImage = (categoryId) => {
  const images = {
    1: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80', // 熱帶水果
    2: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80', // 溫帶水果
    3: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80', // 進口水果
    4: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'  // 禮盒
  }
  return images[categoryId] || 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
}

// Watch 認證狀態變化
watch(
  () => store.state.auth.isAuthenticated,
  (isAuthenticated) => {
    if (isAuthenticated) {
      store.dispatch('wishlist/fetchWishlist')
    } else {
      store.commit('wishlist/SET_WISHLIST', [])
    }
  },
  { immediate: true }
)
</script>

<style scoped>
.card {
  transition: transform 0.2s;
}

.card:hover {
  transform: translateY(-5px);
}

.card-img-top {
  height: 200px;
  object-fit: cover;
}
</style> 