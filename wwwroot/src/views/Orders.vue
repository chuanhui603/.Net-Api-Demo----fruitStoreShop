<template>
  <div class="orders">
    <div class="container py-5">
      <h2 class="mb-4">我的訂單</h2>

      <div v-if="loading" class="text-center py-5">
        <div class="spinner-border text-primary" role="status">
          <span class="visually-hidden">Loading...</span>
        </div>
      </div>

      <div v-else-if="orders.length === 0" class="text-center py-5">
        <i class="bi bi-receipt display-1 text-muted"></i>
        <h3 class="mt-3">您還沒有任何訂單</h3>
        <p class="text-muted mb-4">快去選購您喜歡的商品吧！</p>
        <router-link to="/products" class="btn btn-primary">前往購物</router-link>
      </div>

      <div v-else class="row">
        <div v-for="order in orders" :key="order.id" class="col-12 mb-4">
          <div class="card">
            <div class="card-header d-flex justify-content-between align-items-center">
              <h5 class="mb-0">訂單編號: {{ order.id }}</h5>
              <span class="badge" :class="getStatusClass(order.status)">
                {{ getStatusText(order.status) }}
              </span>
            </div>
            <div class="card-body">
              <div class="row">
                <div class="col-md-8">
                  <div v-for="item in order.items" :key="item.id" class="d-flex mb-3">
                    <img :src="item.imageUrl" class="me-3" style="width: 80px; height: 80px; object-fit: cover;">
                    <div>
                      <h6 class="mb-1">{{ item.name }}</h6>
                      <p class="text-muted mb-0">數量: {{ item.quantity }}</p>
                      <p class="text-primary mb-0">NT$ {{ item.price }}</p>
                    </div>
                  </div>
                </div>
                <div class="col-md-4">
                  <div class="d-flex justify-content-between mb-2">
                    <span>小計</span>
                    <span>NT$ {{ order.totalAmount - order.shippingFee }}</span>
                  </div>
                  <div class="d-flex justify-content-between mb-2">
                    <span>運費</span>
                    <span>NT$ {{ order.shippingFee }}</span>
                  </div>
                  <div class="d-flex justify-content-between mb-3">
                    <strong>總計</strong>
                    <strong>NT$ {{ order.totalAmount }}</strong>
                  </div>
                  <div class="d-flex justify-content-between">
                    <span>付款方式</span>
                    <span>{{ getPaymentMethodText(order.paymentMethod) }}</span>
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
const loading = ref(false)

// Computed
const orders = computed(() => store.state.orders.orders)

// 方法
const getStatusClass = (status) => {
  const classes = {
    pending: 'bg-warning',
    processing: 'bg-info',
    shipped: 'bg-primary',
    completed: 'bg-success',
    cancelled: 'bg-danger'
  }
  return classes[status] || 'bg-secondary'
}

const getStatusText = (status) => {
  const texts = {
    pending: '待處理',
    processing: '處理中',
    shipped: '已出貨',
    completed: '已完成',
    cancelled: '已取消'
  }
  return texts[status] || status
}

const getPaymentMethodText = (method) => {
  const texts = {
    creditCard: '信用卡',
    linePay: 'Line Pay'
  }
  return texts[method] || method
}

// 生命週期
onMounted(async () => {
  loading.value = true
  try {
    await store.dispatch('orders/fetchOrders')
  } catch (error) {
    console.error('Error fetching orders:', error)
  } finally {
    loading.value = false
  }
})
</script>

<style scoped>
.card {
  border: none;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

.badge {
  padding: 0.5rem 1rem;
  font-size: 0.9rem;
}
</style> 