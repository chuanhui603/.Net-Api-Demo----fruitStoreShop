<template>
  <div class="home">
    <!-- Hero Section -->
    <div class="hero-section bg-light py-4 py-md-5">
      <div class="container">
        <div class="row align-items-center">
          <div class="col-12 col-md-6 order-2 order-md-1">
            <h1 class="display-4 mb-3">新鮮水果，直送到家</h1>
            <p class="lead mb-4">精選台灣在地優質水果，讓您享受最新鮮的美味</p>
            <router-link to="/products" class="btn btn-primary btn-lg">立即選購</router-link>
          </div>
          <div class="col-12 col-md-6 order-1 order-md-2 mb-4 mb-md-0">
            <img src="https://images.unsplash.com/photo-1610832958506-aa56368176cf?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80" 
                 alt="新鮮水果" 
                 class="img-fluid rounded shadow">
          </div>
        </div>
      </div>
    </div>

    <!-- Featured Products -->
    <section class="featured-products py-4 py-md-5">
      <div class="container">
        <h2 class="text-center mb-4">熱門商品</h2>
        <div class="row g-3">
          <div v-for="product in featuredProducts.slice(0, 8)" 
               :key="product.id" 
               class="col-6 col-md-3">
            <div class="card h-100">
              <img :src="getProductImage(product.categoryId)" 
                   class="card-img-top" 
                   :alt="product.name">
              <div class="card-body d-flex flex-column">
                <h5 class="card-title">{{ product.name }}</h5>
                <p class="card-text flex-grow-1">{{ product.description }}</p>
                <p class="card-text text-primary fw-bold">NT$ {{ product.price }}</p>
                <button @click="addToCart(product)" 
                        class="btn btn-primary w-100">加入購物車</button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Categories -->
    <section class="categories py-4 py-md-5 bg-light">
      <div class="container">
        <h2 class="text-center mb-4">商品分類</h2>
        <div v-if="loading" class="text-center">
          <div class="spinner-border text-primary" role="status">
            <span class="visually-hidden">Loading...</span>
          </div>
        </div>
        <div v-else-if="categories.length === 0" class="text-center">
          <p>暫無分類數據</p>
        </div>
        <div v-else class="row g-3">
          <div v-for="category in categories.slice(0, 8)" 
               :key="category.id" 
               class="col-6 col-md-3">
            <div class="card h-100">
              <img :src="getCategoryImage(category.id)" 
                   class="card-img-top" 
                   :alt="category.name">
              <div class="card-body text-center d-flex flex-column">
                <h5 class="card-title">{{ category.name }}</h5>
                <router-link :to="'/products?category=' + category.id" 
                            class="btn btn-outline-primary mt-auto">
                  查看商品
                </router-link>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useStore } from 'vuex'

const store = useStore()
const loading = ref(false)

// Computed properties
const featuredProducts = computed(() => store.state.products.featured)
const categories = computed(() => store.state.categories.list)

// Methods
const addToCart = (product) => {
  store.dispatch('cart/addToCart', product)
}

const getCategoryImage = (categoryId) => {
  const images = {
    1: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80', // 熱帶水果
    2: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80', // 溫帶水果
    3: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80', // 進口水果
    4: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'  // 禮盒
  }
  return images[categoryId] || 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
}

const getProductImage = (categoryId) => {
  return getCategoryImage(categoryId)
}

// Load data function
const loadData = async () => {
  loading.value = true
  try {
    await Promise.all([
      store.dispatch('products/fetchFeaturedProducts', { limit: 8 }),
      store.dispatch('categories/fetchCategories')
    ])
  } catch (error) {
    console.error('Error loading data:', error)
  } finally {
    loading.value = false
  }
}

// Lifecycle - 進入頁面時載入資料
onMounted(() => {
  loadData()
})

</script>

<style scoped>
.hero-section {
  margin-bottom: 2rem;
}

.card {
  transition: transform 0.2s;
  border: none;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

.card:hover {
  transform: translateY(-5px);
  box-shadow: 0 4px 8px rgba(0,0,0,0.2);
}

.card-img-top {
  height: 200px;
  object-fit: cover;
}

.categories .card-img-top {
  height: 150px;
  object-fit: cover;
}

@media (max-width: 768px) {
  .hero-section {
    margin-bottom: 1.5rem;
  }
  
  .display-4 {
    font-size: 2rem;
  }
  
  .lead {
    font-size: 1rem;
  }
  
  .card-img-top {
    height: 150px;
  }
  
  .categories .card-img-top {
    height: 120px;
  }
}

@media (max-width: 576px) {
  .card-img-top {
    height: 120px;
  }
  
  .categories .card-img-top {
    height: 100px;
  }
}
</style> 