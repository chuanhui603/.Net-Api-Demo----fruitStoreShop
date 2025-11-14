<template>
  <div class="member">
    <div class="container py-4">
      <div class="row">
        <!-- Sidebar -->
        <div class="col-md-3">
          <div class="card">
            <div class="card-body">
              <div class="text-center mb-3">
                <i class="bi bi-person-circle display-4"></i>
                <h5 class="mt-2">{{ currentUser.name }}</h5>
                <p class="text-muted">{{ currentUser.email }}</p>
              </div>
              <ul class="nav flex-column">
                <li class="nav-item">
                  <router-link to="/member/profile" class="nav-link" active-class="active">
                    <i class="bi bi-person me-2"></i>個人資料
                  </router-link>
                </li>
                <li class="nav-item">
                  <router-link to="/member/orders" class="nav-link" active-class="active">
                    <i class="bi bi-receipt me-2"></i>我的訂單
                  </router-link>
                </li>
                <li class="nav-item">
                  <router-link to="/member/wishlist" class="nav-link" active-class="active">
                    <i class="bi bi-heart me-2"></i>願望清單
                  </router-link>
                </li>
                <li class="nav-item">
                  <router-link to="/member/addresses" class="nav-link" active-class="active">
                    <i class="bi bi-geo-alt me-2"></i>收件地址
                  </router-link>
                </li>
                <li class="nav-item">
                  <router-link to="/member/coupons" class="nav-link" active-class="active">
                    <i class="bi bi-ticket-perforated me-2"></i>優惠券
                  </router-link>
                </li>
                <li class="nav-item">
                  <a href="#" class="nav-link text-danger" @click.prevent="handleLogout">
                    <i class="bi bi-box-arrow-right me-2"></i>登出
                  </a>
                </li>
              </ul>
            </div>
          </div>
        </div>

        <!-- Main Content -->
        <div class="col-md-9">
          <router-view></router-view>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'
import { useStore } from 'vuex'
import { useRouter } from 'vue-router'

const store = useStore()
const router = useRouter()

// Computed
const currentUser = computed(() => store.getters['auth/currentUser'])

// 方法
const handleLogout = async () => {
  await store.dispatch('auth/logout')
  router.push('/login')
}
</script>

<style scoped>
.nav-link {
  color: #333;
  padding: 0.5rem 1rem;
  border-radius: 0.25rem;
}

.nav-link:hover {
  background-color: #f8f9fa;
}

.nav-link.active {
  background-color: #e9ecef;
  font-weight: bold;
}
</style> 