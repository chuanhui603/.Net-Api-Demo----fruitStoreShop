<template>
  <div class="checkout">
    <div class="container py-5">
      <div class="row">
        <!-- 訂單資訊 -->
        <div class="col-md-8">
          <div class="card mb-4">
            <div class="card-header">
              <h4 class="mb-0">訂單資訊</h4>
            </div>
            <div class="card-body">
              <form @submit.prevent="submitOrder">
                <div class="mb-3">
                  <label class="form-label">收件人姓名</label>
                  <input type="text" class="form-control" v-model="orderInfo.name" required>
                </div>
                <div class="mb-3">
                  <label class="form-label">聯絡電話</label>
                  <input type="tel" class="form-control" v-model="orderInfo.phone" required>
                </div>
                <div class="mb-3">
                  <label class="form-label">電子郵件</label>
                  <input type="email" class="form-control" v-model="orderInfo.email" required>
                </div>
                <div class="mb-3">
                  <label class="form-label">收件地址</label>
                  <textarea class="form-control" v-model="orderInfo.address" rows="3" required></textarea>
                </div>
                <div class="mb-3">
                  <label class="form-label">備註</label>
                  <textarea class="form-control" v-model="orderInfo.note" rows="2"></textarea>
                </div>
              </form>
            </div>
          </div>

          <!-- 付款方式 -->
          <div class="card">
            <div class="card-header">
              <h4 class="mb-0">付款方式</h4>
            </div>
            <div class="card-body">
              <div class="form-check mb-3">
                <input class="form-check-input" type="radio" v-model="paymentMethod" value="creditCard" id="creditCard">
                <label class="form-check-label" for="creditCard">
                  信用卡付款
                </label>
              </div>
              <div v-if="paymentMethod === 'creditCard'" class="credit-card-form">
                <div class="mb-3">
                  <label class="form-label">信用卡號碼</label>
                  <input type="text" class="form-control" v-model="creditCardInfo.number" placeholder="1234 5678 9012 3456" required>
                </div>
                <div class="row">
                  <div class="col-md-6 mb-3">
                    <label class="form-label">有效期限</label>
                    <input type="text" class="form-control" v-model="creditCardInfo.expiry" placeholder="MM/YY" required>
                  </div>
                  <div class="col-md-6 mb-3">
                    <label class="form-label">安全碼</label>
                    <input type="text" class="form-control" v-model="creditCardInfo.cvv" placeholder="CVV" required>
                  </div>
                </div>
              </div>

              <div class="form-check">
                <input class="form-check-input" type="radio" v-model="paymentMethod" value="linePay" id="linePay">
                <label class="form-check-label" for="linePay">
                  Line Pay
                </label>
              </div>
            </div>
          </div>
        </div>

        <!-- 訂單摘要 -->
        <div class="col-md-4">
          <div class="card">
            <div class="card-header">
              <h4 class="mb-0">訂單摘要</h4>
            </div>
            <div class="card-body">
              <div class="d-flex justify-content-between mb-2">
                <span>小計</span>
                <span>NT$ {{ totalPrice }}</span>
              </div>
              <div class="d-flex justify-content-between mb-2">
                <span>運費</span>
                <span>NT$ {{ shippingFee }}</span>
              </div>
              <div class="d-flex justify-content-between mb-3">
                <strong>總計</strong>
                <strong>NT$ {{ totalPrice + shippingFee }}</strong>
              </div>
              <button @click="submitOrder" class="btn btn-primary w-100" :disabled="!isFormValid">
                確認付款
              </button>
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
const orderInfo = ref({
  name: '',
  phone: '',
  email: '',
  address: '',
  note: ''
})
const paymentMethod = ref('creditCard')
const creditCardInfo = ref({
  number: '',
  expiry: '',
  cvv: ''
})
const shippingFee = ref(100)

// Computed
const items = computed(() => store.state.cart.items)
const totalPrice = computed(() => 
  items.value.reduce((total, item) => total + (item.price * item.quantity), 0)
)
const isFormValid = computed(() => {
  if (paymentMethod.value === 'creditCard') {
    return orderInfo.value.name && 
           orderInfo.value.phone && 
           orderInfo.value.email && 
           orderInfo.value.address &&
           creditCardInfo.value.number &&
           creditCardInfo.value.expiry &&
           creditCardInfo.value.cvv
  }
  return orderInfo.value.name && 
         orderInfo.value.phone && 
         orderInfo.value.email && 
         orderInfo.value.address
})

// 方法
const submitOrder = async () => {
  if (!isFormValid.value) return

  const orderData = {
    ...orderInfo.value,
    items: items.value,
    totalAmount: totalPrice.value + shippingFee.value,
    paymentMethod: paymentMethod.value,
    creditCardInfo: paymentMethod.value === 'creditCard' ? creditCardInfo.value : null
  }

  try {
    await store.dispatch('orders/createOrder', orderData)
    router.push('/orders')
  } catch (error) {
    console.error('Error creating order:', error)
    alert('訂單建立失敗，請稍後再試')
  }
}
</script>

<style scoped>
.credit-card-form {
  background-color: #f8f9fa;
  padding: 1rem;
  border-radius: 0.25rem;
  margin-bottom: 1rem;
}

.form-check {
  margin-bottom: 1rem;
}

.form-check-input {
  margin-right: 0.5rem;
}

.btn-primary {
  padding: 0.75rem;
  font-size: 1.1rem;
}
</style> 