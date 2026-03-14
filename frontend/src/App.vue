<script setup lang="ts">
import { computed, onMounted, ref, type Ref } from "vue";

const estaLogado = ref(false);
onMounted(() => {
  estaLogado.value = !!localStorage.getItem("token");

});

function logout() {
  localStorage.removeItem("token");
  location.reload();
}
import Login from './components/Login.vue'
import ConsultaCidade from './components/ConsultaCidade.vue'
import ConsultaCoordenadas from "./components/ConsultaCoordenadas.vue";
</script>

<template>
  <div class="h-100 w-100">
    <nav class="navbar navbar-expand-lg bg-body-tertiary" v-if="estaLogado">
      <div class="container-fluid">
        <a class="navbar-brand">Clover</a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent"
          aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarSupportedContent">
          <ul class="navbar-nav me-auto mb-2 mb-lg-0">
            <li class="nav-item">
              <ConsultaCidade></ConsultaCidade>
             </li>
            <li class="nav-item">
              <ConsultaCoordenadas></ConsultaCoordenadas>
            </li>
          </ul>
        </div>
      </div>
      <div class="container-fluid">
        <button class="btn btn-outline-danger ms-auto" @click="logout" type="submit">Sair</button>
      </div>
    </nav>
    <!-- Fim Nav Bar -->

    <Login v-if="!estaLogado" />
  </div>
</template>

<style scoped></style>
