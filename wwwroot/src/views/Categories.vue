<template>
  <div class="categories">
    <div class="container py-4">
      <h2 class="mb-4">商品分類</h2>

      <div v-if="loading" class="text-center py-5">
        <div class="spinner-border text-primary" role="status">
          <span class="visually-hidden">Loading...</span>
        </div>
      </div>

      <div v-else-if="!categories || categories.length === 0" class="text-center py-5">
        <i class="bi bi-grid display-1 text-muted"></i>
        <h3 class="mt-3">暫無分類</h3>
      </div>

      <div v-else class="row">
        <div v-for="category in categories" :key="category.id" class="col-md-4 mb-4">
          <div class="card h-100">
            <img :src="getCategoryImage(category.id)" class="card-img-top" :alt="category.name">
            <div class="card-body">
              <h5 class="card-title">{{ category.name }}</h5>
              <p class="card-text">{{ category.description }}</p>
              <router-link :to="'/products?category=' + category.id" class="btn btn-primary">
                查看商品
              </router-link>
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
const loading = ref(false)

// Computed
const categories = computed(() => store.state.categories.categories)

// 方法
const getCategoryImage = (categoryId) => {
  const images = {
    1: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80', // 熱帶水果
    2: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80', // 溫帶水果
    3: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80', // 進口水果
    4: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'  // 禮盒
  }
  return images[categoryId] || 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
}

// 生命週期
onMounted(async () => {
  loading.value = true
  try {
    await store.dispatch('categories/fetchCategories')
  } catch (error) {
    console.error('Error loading categories:', error)
  } finally {
    loading.value = false
  }
})
</script>

<style scoped>
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

@media (max-width: 768px) {
  .card-img-top {
    height: 150px;
  }
}
</style> 