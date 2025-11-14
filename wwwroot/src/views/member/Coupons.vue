<template>
  <div class="coupons">
    <div class="card">
      <div class="card-body">
        <h5 class="card-title mb-4">我的優惠券</h5>
        
        <div v-if="loading" class="text-center">
          <div class="spinner-border text-primary" role="status">
            <span class="visually-hidden">Loading...</span>
          </div>
        </div>

        <div v-else-if="coupons.length === 0" class="text-center">
          <p>您目前沒有可用的優惠券</p>
        </div>

        <div v-else class="coupon-list">
          <div v-for="coupon in coupons" :key="coupon.id" class="coupon-item mb-3">
            <div class="card">
              <div class="card-body">
                <div class="row align-items-center">
                  <div class="col-md-8">
                    <h6 class="mb-1">{{ coupon.name }}</h6>
                    <p class="mb-1 text-primary">折扣: {{ coupon.discount }}%</p>
                    <p class="mb-1">最低消費: NT$ {{ coupon.minPurchase }}</p>
                    <p class="mb-1">有效期限: {{ formatDate(coupon.expiryDate) }}</p>
                    <p class="mb-0" v-if="coupon.description">{{ coupon.description }}</p>
                  </div>
                  <div class="col-md-4 text-end">
                    <span :class="['badge', getStatusClass(coupon.status)]">
                      {{ getStatusText(coupon.status) }}
                    </span>
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
const coupons = ref([])

// Computed
const user = computed(() => store.state.auth.user)

// 方法
const loadCoupons = async () => {
  if (!user.value) return
  loading.value = true
  try {
    coupons.value = await store.dispatch('fetchCoupons')
  } catch (error) {
    console.error('Failed to fetch coupons:', error)
  } finally {
    loading.value = false
  }
}

const formatDate = (date) => {
  return new Date(date).toLocaleDateString()
}

const getStatusClass = (status) => {
  const statusClasses = {
    active: 'bg-success',
    expired: 'bg-secondary',
    used: 'bg-danger'
  }
  return statusClasses[status] || 'bg-secondary'
}

const getStatusText = (status) => {
  const statusTexts = {
    active: '可使用',
    expired: '已過期',
    used: '已使用'
  }
  return statusTexts[status] || status
}

// 初始化
onMounted(() => {
  loadCoupons()
})
</script>

<style scoped>
.coupon-item {
  transition: transform 0.2s;
}

.coupon-item:hover {
  transform: translateY(-3px);
  box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
}
</style> 