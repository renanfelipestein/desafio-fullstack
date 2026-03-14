<script setup lang="ts">
import axios from "axios";
import { ref } from "vue";
const email = ref<string>("clover@aliare.co");
const password = ref<string>("clover123");
const erro = ref<string>("");
const API_BASE = import.meta.env.VITE_API_URL;

async function login(email: string, password: string) {
  try {
    const { data } = await axios.post(
      `${API_BASE}/api/login`,
      {
        email: email,
        senha: password,
      });
    localStorage.setItem("token", data.token);  
    location.reload();
  } catch (error: string | any) {
    erro.value = error.response?.data?.message;
    setTimeout(() => {
      erro.value = "";
    }, 2500);
  }

}
</script>
<template>
    <div style="width: 300px; height: auto;min-height: 200px;" class="mt-5 container shadow p-3 mb-5 bg-body-tertiary rounded justify-content-center align-items-center d-flex">
      <form class="row g-3">
        <div>
          <div class="col-auto mb-3 mt-3">
            <label for="inputEmail" class="visually-hidden">Email</label>
            <input
              type="email"
              class="form-control"
              id="inputEmail"
              placeholder="Email"
              v-model="email"   
            />
          </div>
          <div class="col-auto mb-3">
            <label for="inputPassword" class="visually-hidden">Password</label>
            <input
              type="password"
              class="form-control"
              id="inputPassword"
              placeholder="Password"
              v-model="password"
            />
          </div>
          <div class="alert alert-danger mt-2" v-if="erro">
            {{erro}}
          </div>
          <div class="col-auto text-center justify-content-center align-items-center d-flex">
            <button class="btn btn-primary mb-3" @click.prevent="login(email, password)" >   
              Entrar
            </button>
          </div>
        </div>
      </form>
    </div>
</template>

    <style scoped></style>