<template>
  <div class="register">
    <div class="container py-5">
      <div class="row justify-content-center">
        <div class="col-md-6">
          <div class="card">
            <div class="card-body">
              <h2 class="text-center mb-4">註冊新帳號</h2>
              <form @submit.prevent="register">
                <div class="row">
                  <div class="col-md-6 mb-3">
                    <label for="firstName" class="form-label">名字</label>
                    <input type="text" v-model="userData.FirstName" class="form-control" id="firstName" required>
                  </div>
                  <div class="col-md-6 mb-3">
                    <label for="lastName" class="form-label">姓氏</label>
                    <input type="text" v-model="userData.LastName" class="form-control" id="lastName" required>
                  </div>
                </div>
                <div class="mb-3">
                  <label for="email" class="form-label">電子郵件</label>
                  <input type="email" v-model="userData.Email" @blur="checkEmailExists" class="form-control" id="email"
                    required>
                  <label v-if="isEmailExists" class="text-danger mt-1" style="font-size:0.95em;">{{ emailMsg
                  }}</label>
                </div>
                <div class="mb-3">
                  <label for="password" class="form-label">密碼</label>
                  <input type="password" v-model="userData.Password" class="form-control" id="password" required>
                </div>
                <div class="mb-3">
                  <label for="confirmPassword" class="form-label">確認密碼</label>
                  <input type="password" v-model="confirmPassword" class="form-control" id="confirmPassword" required>
                </div>
                <div class="mb-3">
                  <label for="phone" class="form-label">電話</label>
                  <input type="tel" v-model="userData.Phone" class="form-control" id="phone" required>
                </div>
                <div class="mb-3">
                  <label for="address" class="form-label">地址</label>
                  <input type="text" v-model="userData.address" class="form-control" id="address" required>
                </div>
                <div class="mb-3">
                  <label for="gender" class="form-label">性別</label>
                  <select v-model="userData.Gender" class="form-select" id="gender" required>
                    <option value="">請選擇</option>
                    <option value="male">男性</option>
                    <option value="female">女性</option>
                    <option value="other">其他</option>
                  </select>
                </div>
                <div class="mb-3 form-check">
                  <input type="checkbox" v-model="agreeTerms" class="form-check-input" id="terms" required>
                  <label class="form-check-label" for="terms">
                    我已閱讀並同意<a href="#" @click.prevent="showTerms">服務條款</a>
                  </label>
                </div>
                <div class="d-grid gap-2">
                  <button type="submit" class="btn btn-primary" :disabled="loading">
                    {{ loading ? '註冊中...' : '註冊' }}
                  </button>
                  <router-link to="/login" class="btn btn-outline-primary">
                    已有帳號？登入
                  </router-link>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useStore } from 'vuex'
import { useRouter } from 'vue-router'

const store = useStore()
const router = useRouter()

// 響應式數據
const userData = ref({
  "FirstName": "",
  "LastName": "",
  "address": "",
  "Phone": "",
  "Email": "",
  "Gender": "",
  "Password": ""
})
const confirmPassword = ref('')
const agreeTerms = ref(false)
const loading = ref(false)
const emailMsg = ref('');
const isEmailExists = ref(false);


const checkEmailExists = async () => {
  emailMsg.value = '';
  isEmailExists.value = false;
  if (!userData.value.Email) {
    emailMsg.value = '請輸入Email';
    isEmailExists.value = true;
    return;
  }
  try {
    const exists = await store.dispatch('auth/checkUserExists', userData.value.Email)
    if (!exists) {
      isEmailExists.value = false;
      return;
    }
  } catch (error) {
    console.error(error);
    emailMsg.value = `系統發生錯誤，${error}`;
  }
  emailMsg.value = '此電子郵件已被註冊';
  isEmailExists.value = true;
  return;
}

// 方法
const register = async () => {
  if (userData.value.Password !== confirmPassword.value) {
    alert('密碼不一致')
    return
  }

  if (isEmailExists.value == true) {
    alert('此電子郵件已被註冊');
    return;
  }

  if (!agreeTerms.value) {
    alert('請同意服務條款')
    return
  }

  loading.value = true
  try {
    await store.dispatch('auth/register', userData.value)
    router.push('/login')
  } catch (error) {
    alert('註冊失敗，請稍後再試')
  } finally {
    loading.value = false
  }
}

const showTerms = () => {
  // 顯示服務條款
  alert('服務條款內容...')
}
</script>

<style scoped>
.card {
  max-width: 500px;
  margin: 0 auto;
}
</style>