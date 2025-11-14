<template>
  <div class="app-container">
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
      <div class="container">
        <router-link class="navbar-brand" to="/">水水水果</router-link>
        <button class="navbar-toggler" type="button" @click="toggleNavbar">
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="navbar-collapse" :class="{ 'show': isNavbarOpen }" id="navbarNav">
          <ul class="navbar-nav me-auto">
            <li class="nav-item">
              <router-link class="nav-link" to="/products">商品列表</router-link>
            </li>
            <li class="nav-item">
              <router-link class="nav-link" to="/categories">商品分類</router-link>
            </li>
          </ul>
          <div class="d-flex">
            <router-link class="btn btn-outline-primary me-2" to="/cart">
              <i class="bi bi-cart"></i> 購物車
            </router-link>
            <router-link class="btn btn-outline-primary me-2" to="/wishlist">
              <i class="bi bi-heart"></i> 願望清單
            </router-link>
            <template v-if="isAuthenticated">
              <router-link class="btn btn-outline-primary me-2" to="/orders">
                <i class="bi bi-receipt"></i> 我的訂單
              </router-link>
              <div class="dropdown">
                <button class="btn btn-outline-primary dropdown-toggle" type="button" @click="toggleUserDropdown">
                  <i class="bi bi-person"></i> {{ currentUser.name }}
                </button>
                <ul class="dropdown-menu" :class="{ 'show': isUserDropdownOpen }" style="right: 0; left: auto;">
                  <li>
                    <div class="dropdown-header">
                      <strong>{{ currentUser.name }}</strong>
                      <p class="text-muted mb-0">{{ currentUser.email }}</p>
                      <p class="text-muted mb-0">{{ currentUser.phone }}</p>
                      <p class="text-muted mb-0">{{ currentUser.address }}</p>
                    </div>
                  </li>
                  <li><hr class="dropdown-divider"></li>
                  <li>
                    <router-link class="dropdown-item" to="/member">
                      <i class="bi bi-person-circle me-2"></i>會員中心
                    </router-link>
                  </li>
                  <li><hr class="dropdown-divider"></li>
                  <li>
                    <a class="dropdown-item text-danger" href="#" @click.prevent="handleLogout">
                      <i class="bi bi-box-arrow-right me-2"></i>登出
                    </a>
                  </li>
                </ul>
              </div>
            </template>
            <template v-else>
              <router-link class="btn btn-outline-primary" to="/login">
                <i class="bi bi-person"></i> 登入
              </router-link>
            </template>
          </div>
        </div>
      </div>
    </nav>

    <main class="container mt-4">
      <router-view></router-view>
    </main>

    <footer class="bg-light mt-5 py-3">
      <div class="container text-center">
        <p class="mb-0">© 2024 水水水果電商. All rights reserved.</p>
      </div>
    </footer>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, onBeforeUnmount, watch } from 'vue'
import { useStore } from 'vuex'
import { useRouter, useRoute } from 'vue-router'

const store = useStore()
const router = useRouter()
const route = useRoute()

// 響應式數據
const isNavbarOpen = ref(false)
const isUserDropdownOpen = ref(false)

// Computed
const isAuthenticated = computed(() => store.getters['auth/isAuthenticated'])
const currentUser = computed(() => store.getters['auth/currentUser'])

// 方法
const toggleNavbar = () => {
  isNavbarOpen.value = !isNavbarOpen.value
}

const toggleUserDropdown = () => {
  isUserDropdownOpen.value = !isUserDropdownOpen.value
}

const handleLogout = () => {
  store.dispatch('auth/logout')
  isUserDropdownOpen.value = false
  router.push('/login')
}

const closeDropdown = (e) => {
  if (!e.target.closest('.dropdown')) {
    isUserDropdownOpen.value = false
  }
}

// Watch 路由變化
watch(() => route.path, () => {
  // 路由變化時關閉導航欄和下拉選單
  isNavbarOpen.value = false
  isUserDropdownOpen.value = false
})

// 生命週期
onMounted(() => {
  // 點擊其他地方時關閉下拉選單
  document.addEventListener('click', closeDropdown)
})

onBeforeUnmount(() => {
  document.removeEventListener('click', closeDropdown)
})
</script>

<style>
.app-container {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

main {
  flex: 1;
}

.dropdown-menu {
  position: absolute;
  min-width: 250px;
  padding: 1rem;
}

.dropdown-header {
  padding: 0.5rem 1rem;
  white-space: normal;
}

.dropdown-item {
  padding: 0.5rem 1rem;
  display: flex;
  align-items: center;
}

.dropdown-item:hover {
  background-color: #f8f9fa;
}

@media (max-width: 991.98px) {
  .navbar-collapse {
    position: absolute;
    top: 100%;
    left: 0;
    right: 0;
    background-color: #f8f9fa;
    padding: 1rem;
    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    z-index: 1000;
  }
  
  .navbar-collapse:not(.show) {
    display: none;
  }
  
  .navbar-nav {
    margin-bottom: 1rem;
  }
  
  .d-flex {
    flex-direction: column;
    gap: 0.5rem;
  }
  
  .btn {
    width: 100%;
  }

  .dropdown-menu {
    position: static !important;
    transform: none !important;
    width: 100%;
    margin-top: 0.5rem;
    box-shadow: none;
  }
}
</style> 