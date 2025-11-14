<template>
  <div class="products">
    <div class="container py-4">
      <div class="row">
        <!-- Filters Sidebar -->
        <div class="col-md-3">
          <div class="card mb-4">
            <div class="card-header">
              <h5 class="mb-0">篩選條件</h5>
            </div>
            <div class="card-body">
              <div class="mb-3">
                <label class="form-label">價格範圍</label>
                <div class="d-flex align-items-center">
                  <input type="number" v-model="filters.minPrice" class="form-control me-2" placeholder="最低">
                  <span>-</span>
                  <input type="number" v-model="filters.maxPrice" class="form-control ms-2" placeholder="最高">
                </div>
              </div>
              <div class="mb-3">
                <label class="form-label">分類</label>
                <select v-model="filters.category" class="form-select">
                  <option value="">全部</option>
                  <option v-for="cat in categories" :key="cat.id" :value="cat.id">
                    {{ cat.name }}
                  </option>
                </select>
              </div>
              <div class="mb-3">
                <label class="form-label">排序</label>
                <select v-model="filters.sortBy" class="form-select">
                  <option value="price_asc">價格由低到高</option>
                  <option value="price_desc">價格由高到低</option>
                  <option value="name_asc">名稱 A-Z</option>
                  <option value="name_desc">名稱 Z-A</option>
                </select>
              </div>
              <button @click="applyFilters" class="btn btn-primary w-100">套用篩選</button>
            </div>
          </div>
        </div>

        <!-- Products Grid -->
        <div class="col-md-9">
          <div class="d-flex justify-content-between align-items-center mb-4">
            <h2>商品列表</h2>
            <div class="input-group" style="width: 300px;">
              <input type="text" v-model="searchQuery" class="form-control" placeholder="搜尋商品...">
              <button class="btn btn-outline-primary" @click="searchProducts">
                <i class="bi bi-search"></i>
              </button>
            </div>
          </div>

          <div class="row">
            <div v-for="product in products" :key="product.id" class="col-md-4 mb-4">
              <div class="card h-100">
                <img :src="product.imageUrl" class="card-img-top" :alt="product.name">
                <div class="card-body">
                  <h5 class="card-title">{{ product.name }}</h5>
                  <p class="card-text">{{ product.description }}</p>
                  <p class="card-text text-primary">NT$ {{ product.price }}</p>
                  <div class="d-flex justify-content-between">
                    <router-link :to="'/products/' + product.id" class="btn btn-outline-primary">
                      查看詳情
                    </router-link>
                    <button @click="addToCart(product)" class="btn btn-primary">
                      加入購物車
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- Pagination -->
          <nav v-if="totalPages > 1" class="mt-4">
            <ul class="pagination justify-content-center">
              <li class="page-item" :class="{ disabled: currentPage === 1 }">
                <a class="page-link" href="#" @click.prevent="changePage(currentPage - 1)">上一頁</a>
              </li>
              <li v-for="page in totalPages" :key="page" class="page-item" :class="{ active: currentPage === page }">
                <a class="page-link" href="#" @click.prevent="changePage(page)">{{ page }}</a>
              </li>
              <li class="page-item" :class="{ disabled: currentPage === totalPages }">
                <a class="page-link" href="#" @click.prevent="changePage(currentPage + 1)">下一頁</a>
              </li>
            </ul>
          </nav>
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
const filters = ref({
  minPrice: '',
  maxPrice: '',
  category: '',
  sortBy: 'price_asc'
})
const searchQuery = ref('')
const currentPage = ref(1)
const pageSize = ref(9)

// Computed
const products = computed(() => store.state.products.list)
const categories = computed(() => store.state.categories.list)
const totalProducts = computed(() => store.state.products.total)
const totalPages = computed(() => Math.ceil(totalProducts.value / pageSize.value))

// 方法
const addToCart = (product) => {
  store.dispatch('cart/addToCart', product)
}

const fetchProducts = async () => {
  const params = {
    page: currentPage.value,
    pageSize: pageSize.value,
    ...filters.value,
    search: searchQuery.value
  }
  await store.dispatch('products/fetchProducts', params)
}

const applyFilters = async () => {
  currentPage.value = 1
  await fetchProducts()
}

const searchProducts = async () => {
  currentPage.value = 1
  await fetchProducts()
}

const changePage = async (page) => {
  currentPage.value = page
  await fetchProducts()
}

// 生命週期
onMounted(async () => {
  await fetchProducts()
  await store.dispatch('categories/fetchCategories')
})
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