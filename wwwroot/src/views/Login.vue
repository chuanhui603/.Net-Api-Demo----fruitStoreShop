<template>
  <div class="login">
    <div class="container py-5">
      <div class="row justify-content-center">
        <div class="col-md-6">
          <div class="card">
            <div class="card-body">
              <h2 class="text-center mb-4">登入</h2>
              <form @submit.prevent="login">
                <div class="mb-3">
                  <label for="email" class="form-label">電子郵件</label>
                  <input type="email" v-model="credentials.email" class="form-control" id="email" required>
                </div>
                <div class="mb-3">
                  <label for="password" class="form-label">密碼</label>
                  <input type="password" v-model="credentials.password" class="form-control" id="password" required>
                </div>
                <div class="mb-3 form-check">
                  <input type="checkbox" v-model="rememberMe" class="form-check-input" id="remember">
                  <label class="form-check-label" for="remember">記住我</label>
                </div>
                <div class="d-grid gap-2">
                  <button type="submit" class="btn btn-primary" :disabled="loading">
                    {{ loading ? '登入中...' : '登入' }}
                  </button>
                  <router-link to="/register" class="btn btn-outline-primary">
                    註冊新帳號
                  </router-link>
                </div>
              </form>
              
              <!-- OAuth2 登入分隔線 -->
              <div class="divider my-4">
                <hr>
                <span class="divider-text">或使用以下方式登入</span>
                <hr>
              </div>
              
              <!-- OAuth2 登入按鈕 -->
              <div class="oauth-login">
                <button @click="loginWithGoogle" class="btn btn-google mb-2 w-100" :disabled="oauthLoading.google">
                  <svg width="20" height="20" viewBox="0 0 24 24" class="me-2">
                    <path fill="#4285F4" d="M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z"/>
                    <path fill="#34A853" d="M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z"/>
                    <path fill="#FBBC05" d="M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.93l2.85-2.22.81-.62z"/>
                    <path fill="#EA4335" d="M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.07l3.66 2.84c.87-2.6 3.3-4.53 6.16-4.53z"/>
                  </svg>
                  {{ oauthLoading.google ? '連接中...' : '使用 Google 登入' }}
                </button>
                
                <button @click="loginWithFacebook" class="btn btn-facebook mb-2 w-100" :disabled="oauthLoading.facebook">
                  <svg width="20" height="20" viewBox="0 0 24 24" class="me-2">
                    <path fill="#1877F2" d="M24 12.073c0-6.627-5.373-12-12-12s-12 5.373-12 12c0 5.99 4.388 10.954 10.125 11.854v-8.385H7.078v-3.47h3.047V9.43c0-3.007 1.792-4.669 4.533-4.669 1.312 0 2.686.235 2.686.235v2.953H15.83c-1.491 0-1.956.925-1.956 1.874v2.25h3.328l-.532 3.47h-2.796v8.385C19.612 23.027 24 18.062 24 12.073z"/>
                  </svg>
                  {{ oauthLoading.facebook ? '連接中...' : '使用 Facebook 登入' }}
                </button>
                
                <button @click="loginWithApple" class="btn btn-apple mb-2 w-100" :disabled="oauthLoading.apple">
                  <svg width="20" height="20" viewBox="0 0 24 24" class="me-2">
                    <path fill="currentColor" d="M12.152 6.896c-.948 0-2.415-1.078-3.96-1.04-2.04.027-3.91 1.183-4.961 3.014-2.117 3.675-.546 9.103 1.519 12.09 1.013 1.454 2.208 3.09 3.792 3.039 1.52-.065 2.09-.987 3.935-.987 1.831 0 2.35.987 3.96.948 1.637-.026 2.676-1.48 3.676-2.948 1.156-1.688 1.636-3.325 1.662-3.415-.039-.013-3.182-1.221-3.22-4.857-.026-3.04 2.48-4.494 2.597-4.559-1.429-2.09-3.623-2.324-4.39-2.376-2-.156-3.675 1.09-4.61 1.09zM15.53 3.83c.843-1.012 1.4-2.427 1.245-3.83-1.207.052-2.662.805-3.532 1.818-.78.896-1.454 2.338-1.273 3.714 1.338.104 2.715-.688 3.559-1.701"/>
                  </svg>
                  {{ oauthLoading.apple ? '連接中...' : '使用 Apple 登入' }}
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
import { ref } from 'vue'
import { useStore } from 'vuex'
import { useRoute, useRouter } from 'vue-router'

const store = useStore()
const route = useRoute()
const router = useRouter()

// 響應式數據
const credentials = ref({
  email: '',
  password: ''
})
const rememberMe = ref(false)
const loading = ref(false)
const oauthLoading = ref({
  google: false,
  facebook: false,
  apple: false
})

// 登入方法
const login = async () => {
  loading.value = true
  try {
    await store.dispatch('auth/login', credentials.value)
    // 如果是從購物車來的，登入後返回結帳頁面
    const returnUrl = route.query.returnUrl || '/'
    router.push(returnUrl)
  } catch (error) {
    alert('登入失敗，請檢查您的帳號密碼')
  } finally {
    loading.value = false
  }
}

// OAuth2 登入方法
const loginWithGoogle = async () => {
  oauthLoading.value.google = true
  try {
    // TODO: 實作 Google OAuth2 登入
    console.log('Google OAuth2 登入')
    // 這裡將來會加入實際的 Google OAuth2 邏輯
    alert('Google 登入功能開發中...')
  } catch (error) {
    console.error('Google 登入失敗:', error)
    alert('Google 登入失敗')
  } finally {
    oauthLoading.value.google = false
  }
}

const loginWithFacebook = async () => {
  oauthLoading.value.facebook = true
  try {
    // TODO: 實作 Facebook OAuth2 登入
    console.log('Facebook OAuth2 登入')
    // 這裡將來會加入實際的 Facebook OAuth2 邏輯
    alert('Facebook 登入功能開發中...')
  } catch (error) {
    console.error('Facebook 登入失敗:', error)
    alert('Facebook 登入失敗')
  } finally {
    oauthLoading.value.facebook = false
  }
}

const loginWithApple = async () => {
  oauthLoading.value.apple = true
  try {
    // TODO: 實作 Apple OAuth2 登入
    console.log('Apple OAuth2 登入')
    // 這裡將來會加入實際的 Apple OAuth2 邏輯
    alert('Apple 登入功能開發中...')
  } catch (error) {
    console.error('Apple 登入失敗:', error)
    alert('Apple 登入失敗')
  } finally {
    oauthLoading.value.apple = false
  }
}
</script>

<style scoped>
.card {
  max-width: 400px;
  margin: 0 auto;
}

/* 分隔線樣式 */
.divider {
  position: relative;
  text-align: center;
}

.divider hr {
  margin: 0;
  border-top: 1px solid #dee2e6;
}

.divider-text {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  background: white;
  padding: 0 15px;
  color: #6c757d;
  font-size: 0.875rem;
}

/* OAuth2 按鈕樣式 */
.oauth-login {
  margin-top: 1rem;
}

.btn-google {
  background-color: #fff;
  border: 1px solid #dadce0;
  color: #3c4043;
  transition: all 0.2s ease;
}

.btn-google:hover {
  background-color: #f8f9fa;
  border-color: #dadce0;
  color: #3c4043;
  box-shadow: 0 1px 3px rgba(0,0,0,0.1);
}

.btn-facebook {
  background-color: #1877F2;
  border: 1px solid #1877F2;
  color: white;
  transition: all 0.2s ease;
}

.btn-facebook:hover {
  background-color: #166fe5;
  border-color: #166fe5;
  color: white;
  box-shadow: 0 1px 3px rgba(24,119,242,0.3);
}

.btn-apple {
  background-color: #000;
  border: 1px solid #000;
  color: white;
  transition: all 0.2s ease;
}

.btn-apple:hover {
  background-color: #333;
  border-color: #333;
  color: white;
  box-shadow: 0 1px 3px rgba(0,0,0,0.3);
}

/* 按鈕通用樣式 */
.btn-google, .btn-facebook, .btn-apple {
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 12px 20px;
  font-weight: 500;
  border-radius: 8px;
  text-decoration: none;
}

.btn-google:disabled, .btn-facebook:disabled, .btn-apple:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

/* SVG 圖標樣式 */
.btn svg {
  flex-shrink: 0;
}
</style> 