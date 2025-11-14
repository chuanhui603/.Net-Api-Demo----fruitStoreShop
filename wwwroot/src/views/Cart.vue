<template>
  <div class="cart">
    <div class="container py-5">
      <h2 class="mb-4">購物車</h2>
      
      <div v-if="cartItems.length === 0" class="text-center py-5">
        <h3>您的購物車是空的</h3>
        <p class="text-muted">快去選購您喜歡的商品吧！</p>
        <router-link to="/products" class="btn btn-primary">前往購物</router-link>
      </div>

      <div v-else class="row">
        <div class="col-md-8">
          <div class="card">
            <div class="card-body">
              <div v-for="item in cartItems" :key="item.id" class="cart-item mb-3 pb-3 border-bottom">
                <div class="row align-items-center">
                  <div class="col-md-2">
                    <img :src="getProductImage(item.categoryId)" class="img-fluid" :alt="item.name">
                  </div>
                  <div class="col-md-4">
                    <h5 class="mb-1">{{ item.name }}</h5>
                    <p class="text-muted mb-0">{{ item.description }}</p>
                  </div>
                  <div class="col-md-2">
                    <p class="mb-0">NT$ {{ item.price }}</p>
                  </div>
                  <div class="col-md-2">
                    <div class="input-group">
                      <button class="btn btn-outline-secondary" @click="updateQuantity(item, item.quantity - 1)">-</button>
                      <input type="number" class="form-control text-center" v-model="item.quantity" min="1" @change="updateQuantity(item, item.quantity)">
                      <button class="btn btn-outline-secondary" @click="updateQuantity(item, item.quantity + 1)">+</button>
                    </div>
                  </div>
                  <div class="col-md-2 text-end">
                    <button class="btn btn-outline-danger" @click="removeFromCart(item)">
                      <i class="bi bi-trash"></i>
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div class="col-md-4">
          <div class="card">
            <div class="card-body">
              <h5 class="card-title">訂單摘要</h5>
              <div class="d-flex justify-content-between mb-2">
                <span>小計</span>
                <span>NT$ {{ totalPrice }}</span>
              </div>
              <div class="d-flex justify-content-between mb-2">
                <span>運費</span>
                <span>NT$ {{ shippingFee }}</span>
              </div>
              <div class="d-flex justify-content-between mb-2">
                <span>折扣</span>
                <span>-NT$ {{ discount }}</span>
              </div>
              <div class="input-group mb-3">
                <input type="text" class="form-control" v-model="couponCode" placeholder="輸入優惠碼">
                <button class="btn btn-outline-primary" @click="applyCoupon">套用</button>
              </div>
              <div class="d-flex justify-content-between mb-3">
                <strong>總計</strong>
                <strong>NT$ {{ totalPrice + shippingFee - discount }}</strong>
              </div>
              <button class="btn btn-primary w-100" @click="checkout">前往結帳</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useStore } from 'vuex'
import { useRouter } from 'vue-router'

const store = useStore()
const router = useRouter()

// 響應式數據
const couponCode = ref('')
const shippingFee = ref(100)
const discount = ref(0)

// Computed
const cartItems = computed(() => store.state.cart.items || [])
const totalPrice = computed(() => 
  cartItems.value.reduce((total, item) => total + (item.price * item.quantity), 0)
)

// 方法
const getProductImage = (categoryId) => {
  const images = {
    1: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80', // 熱帶水果
    2: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80', // 溫帶水果
    3: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80', // 進口水果
    4: 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'  // 禮盒
  }
  return images[categoryId] || 'https://images.unsplash.com/photo-1567306226416-28f0efdc88ce?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1000&q=80'
}

const updateQuantity = (item, quantity) => {
  if (quantity < 1) return
  store.dispatch('cart/updateCartItem', { ...item, quantity })
}

const removeFromCart = (item) => {
  store.dispatch('cart/removeFromCart', item)
}

const applyCoupon = async () => {
  try {
    const response = await store.dispatch('cart/applyCoupon', couponCode.value)
    discount.value = response.discount
    alert('優惠碼已套用！')
  } catch (error) {
    alert(error.message)
  }
}

const checkout = () => {
  if (!store.state.auth.isAuthenticated) {
    router.push({
      path: '/login',
      query: { returnUrl: '/checkout' }
    })
    return
  }
  router.push('/checkout')
}
</script>

<style scoped>
.cart-item img {
  height: 100px;
  object-fit: cover;
}
</style> 