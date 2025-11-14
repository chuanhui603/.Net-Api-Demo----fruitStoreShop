<template>
  <div class="profile">
    <div class="card">
      <div class="card-body">
        <h5 class="card-title mb-4">個人資料</h5>
        <form @submit.prevent="updateProfile">
          <div class="row">
            <div class="col-md-6 mb-3">
              <label class="form-label">名字</label>
              <input type="text" v-model="profileData.FirstName" class="form-control" required>
            </div>
            <div class="col-md-6 mb-3">
              <label class="form-label">姓氏</label>
              <input type="text" v-model="profileData.LastName" class="form-control" required>
            </div>
          </div>
          <div class="mb-3">
            <label class="form-label">電子郵件</label>
            <input type="email" v-model="profileData.Email" class="form-control" required>
          </div>
          <div class="mb-3">
            <label class="form-label">電話</label>
            <input type="tel" v-model="profileData.Phone" class="form-control" required>
          </div>
          <div class="mb-3">
            <label class="form-label">地址</label>
            <textarea v-model="profileData.address" class="form-control" rows="3" required></textarea>
          </div>
          <div class="mb-3">
            <label class="form-label">性別</label>
            <select v-model="profileData.Gender" class="form-select" required>
              <option value="">請選擇</option>
              <option value="male">男性</option>
              <option value="female">女性</option>
              <option value="other">其他</option>
            </select>
          </div>
          <button type="submit" class="btn btn-primary" :disabled="loading">
            {{ loading ? '儲存中...' : '儲存變更' }}
          </button>
        </form>

        <hr class="my-4">

        <h5 class="card-title mb-4">變更密碼</h5>
        <form @submit.prevent="changePassword">
          <div class="mb-3">
            <label class="form-label">目前密碼</label>
            <input type="password" v-model="passwordData.currentPassword" class="form-control" required>
          </div>
          <div class="mb-3">
            <label class="form-label">新密碼</label>
            <input type="password" v-model="passwordData.newPassword" class="form-control" required>
          </div>
          <div class="mb-3">
            <label class="form-label">確認新密碼</label>
            <input type="password" v-model="passwordData.confirmPassword" class="form-control" required>
          </div>
          <button type="submit" class="btn btn-primary" :disabled="passwordLoading">
            {{ passwordLoading ? '變更中...' : '變更密碼' }}
          </button>
        </form>
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
const passwordLoading = ref(false)
const profileData = ref({
  FirstName: '',
  LastName: '',
  Email: '',
  Phone: '',
  address: '',
  Gender: ''
})
const passwordData = ref({
  currentPassword: '',
  newPassword: '',
  confirmPassword: ''
})

// Computed
const user = computed(() => store.state.auth.user)

// 方法
const updateProfile = async () => {
  if (loading.value) return
  loading.value = true
  try {
    await store.dispatch('auth/updateProfile', profileData.value)
    alert('個人資料已更新')
  } catch (error) {
    alert('更新失敗，請稍後再試')
  } finally {
    loading.value = false
  }
}

const changePassword = async () => {
  if (passwordLoading.value) return
  if (passwordData.value.newPassword !== passwordData.value.confirmPassword) {
    alert('新密碼與確認密碼不符')
    return
  }
  passwordLoading.value = true
  try {
    await store.dispatch('auth/changePassword', passwordData.value)
    passwordData.value = {
      currentPassword: '',
      newPassword: '',
      confirmPassword: ''
    }
    alert('密碼已變更')
  } catch (error) {
    alert('密碼變更失敗，請確認目前密碼是否正確')
  } finally {
    passwordLoading.value = false
  }
}

// 初始化用戶資料
onMounted(() => {
  if (user.value) {
    profileData.value = {
      FirstName: user.value.FirstName || '',
      LastName: user.value.LastName || '',
      Email: user.value.Email || '',
      Phone: user.value.Phone || '',
      address: user.value.address || '',
      Gender: user.value.Gender || ''
    }
  }
})
</script>

<style scoped>
.card {
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}
</style> 