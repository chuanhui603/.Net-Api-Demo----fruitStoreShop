<template>
  <div class="addresses">
    <div class="card">
      <div class="card-body">
        <h5 class="card-title mb-4">收件地址</h5>
        
        <div v-if="loading" class="text-center">
          <div class="spinner-border text-primary" role="status">
            <span class="visually-hidden">Loading...</span>
          </div>
        </div>

        <div v-else>
          <div class="d-flex justify-content-end mb-3">
            <button class="btn btn-primary" @click="showAddAddressModal">
              <i class="bi bi-plus-lg"></i> 新增地址
            </button>
          </div>

          <div v-if="addresses.length === 0" class="text-center">
            <p>您尚未新增任何收件地址</p>
          </div>

          <div v-else class="address-list">
            <div v-for="address in addresses" :key="address.id" class="address-item mb-3">
              <div class="card">
                <div class="card-body">
                  <div class="d-flex justify-content-between align-items-start">
                    <div>
                      <h6 class="mb-1">{{ address.name }}</h6>
                      <p class="mb-1">{{ address.phone }}</p>
                      <p class="mb-1">{{ address.address }}</p>
                      <p class="mb-0">{{ address.postalCode }}</p>
                    </div>
                    <div class="btn-group">
                      <button class="btn btn-sm btn-outline-primary" @click="editAddress(address)">
                        編輯
                      </button>
                      <button class="btn btn-sm btn-outline-danger" @click="deleteAddress(address.id)">
                        刪除
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Add/Edit Address Modal -->
    <div class="modal fade" id="addressModal" tabindex="-1">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">{{ isEditing ? '編輯地址' : '新增地址' }}</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
          </div>
          <div class="modal-body">
            <form @submit.prevent="saveAddress">
              <div class="mb-3">
                <label class="form-label">收件人姓名</label>
                <input type="text" v-model="addressForm.name" class="form-control" required>
              </div>
              <div class="mb-3">
                <label class="form-label">電話</label>
                <input type="tel" v-model="addressForm.phone" class="form-control" required>
              </div>
              <div class="mb-3">
                <label class="form-label">郵遞區號</label>
                <input type="text" v-model="addressForm.postalCode" class="form-control" required>
              </div>
              <div class="mb-3">
                <label class="form-label">地址</label>
                <textarea v-model="addressForm.address" class="form-control" rows="3" required></textarea>
              </div>
              <div class="mb-3">
                <div class="form-check">
                  <input type="checkbox" v-model="addressForm.isDefault" class="form-check-input" id="defaultAddress">
                  <label class="form-check-label" for="defaultAddress">設為預設地址</label>
                </div>
              </div>
              <div class="text-end">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">取消</button>
                <button type="submit" class="btn btn-primary" :disabled="saving">
                  {{ saving ? '儲存中...' : '儲存' }}
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, nextTick } from 'vue'
import { useStore } from 'vuex'

const store = useStore()

const loading = ref(false)
const saving = ref(false)
const isEditing = ref(false)
const addresses = ref([])
const addressForm = ref({
  id: null,
  name: '',
  phone: '',
  postalCode: '',
  address: '',
  isDefault: false
})

// Computed
const user = computed(() => store.state.auth.user)

// 方法
const loadAddresses = async () => {
  if (!user.value) return
  loading.value = true
  try {
    addresses.value = await store.dispatch('fetchAddresses')
  } catch (error) {
    console.error('Failed to fetch addresses:', error)
  } finally {
    loading.value = false
  }
}

const showAddAddressModal = () => {
  isEditing.value = false
  addressForm.value = {
    id: null,
    name: '',
    phone: '',
    postalCode: '',
    address: '',
    isDefault: false
  }
  nextTick(() => {
    new bootstrap.Modal(document.getElementById('addressModal')).show()
  })
}

const editAddress = (address) => {
  isEditing.value = true
  addressForm.value = { ...address }
  nextTick(() => {
    new bootstrap.Modal(document.getElementById('addressModal')).show()
  })
}

const saveAddress = async () => {
  if (saving.value) return
  saving.value = true
  try {
    if (isEditing.value) {
      await store.dispatch('updateAddress', addressForm.value)
    } else {
      await store.dispatch('addAddress', addressForm.value)
    }
    await loadAddresses()
    bootstrap.Modal.getInstance(document.getElementById('addressModal')).hide()
  } catch (error) {
    alert('儲存失敗，請稍後再試')
  } finally {
    saving.value = false
  }
}

const deleteAddress = async (addressId) => {
  if (confirm('確定要刪除此地址嗎？')) {
    try {
      await store.dispatch('deleteAddress', addressId)
      await loadAddresses()
    } catch (error) {
      alert('刪除失敗，請稍後再試')
    }
  }
}

// 初始化
onMounted(() => {
  loadAddresses()
})
</script>

<style scoped>
.address-item {
  transition: transform 0.2s;
}

.address-item:hover {
  transform: translateX(5px);
}
</style> 