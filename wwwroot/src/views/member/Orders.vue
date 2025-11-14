<template>
  <div class="orders">
    <div class="card">
      <div class="card-body">
        <h5 class="card-title mb-4">我的訂單</h5>
        
        <div v-if="loading" class="text-center">
          <div class="spinner-border text-primary" role="status">
            <span class="visually-hidden">Loading...</span>
          </div>
        </div>

        <div v-else-if="orders.length === 0" class="text-center">
          <p>您目前沒有訂單記錄</p>
        </div>

        <div v-else class="order-list">
          <div v-for="order in orders" :key="order.id" class="order-item mb-4">
            <div class="d-flex justify-content-between align-items-center mb-2">
              <h6 class="mb-0">訂單編號: {{ order.orderNumber }}</h6>
              <span :class="['badge', getStatusClass(order.status)]">
                {{ getStatusText(order.status) }}
              </span>
            </div>
            <div class="order-details">
              <div class="row">
                <div class="col-md-6">
                  <p class="mb-1">訂單日期: {{ formatDate(order.orderDate) }}</p>
                  <p class="mb-1">總金額: NT$ {{ order.totalAmount }}</p>
                </div>
                <div class="col-md-6">
                  <p class="mb-1">付款方式: {{ order.paymentMethod }}</p>
                  <p class="mb-1">配送方式: {{ order.shippingMethod }}</p>
                </div>
              </div>
              <div class="mt-2">
                <button class="btn btn-sm btn-outline-primary" @click="viewOrderDetails(order.id)">
                  查看詳情
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
import { ref, computed, onMounted } from 'vue'
import { useStore } from 'vuex'
import { useRouter } from 'vue-router'

const store = useStore()
const router = useRouter()

// 響應式數據
const loading = ref(false)
const orders = ref([])

// Computed
const user = computed(() => store.state.auth.user)

// 方法
const loadOrders = async () => {
  if (!user.value) return
  loading.value = true
  try {
    orders.value = await store.dispatch('orders/fetchOrders')
  } catch (error) {
    console.error('Failed to fetch orders:', error)
  } finally {
    loading.value = false
  }
}

const formatDate = (date) => {
  return new Date(date).toLocaleDateString()
}

const getStatusClass = (status) => {
  const statusClasses = {
    pending: 'bg-warning',
    processing: 'bg-info',
    shipped: 'bg-primary',
    delivered: 'bg-success',
    cancelled: 'bg-danger'
  }
  return statusClasses[status] || 'bg-secondary'
}

const getStatusText = (status) => {
  const statusTexts = {
    pending: '待處理',
    processing: '處理中',
    shipped: '已出貨',
    delivered: '已送達',
    cancelled: '已取消'
  }
  return statusTexts[status] || status
}

const viewOrderDetails = (orderId) => {
  router.push(`/member/orders/${orderId}`)
}

// 初始化
onMounted(() => {
  loadOrders()
})
</script>

<style scoped>
.order-item {
  border: 1px solid #dee2e6;
  border-radius: 0.25rem;
  padding: 1rem;
}

.order-item:hover {
  box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
}
</style> 