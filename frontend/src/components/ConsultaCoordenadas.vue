<script setup lang="ts">
import { computed, onMounted, reactive, ref, watch, type Ref } from "vue";
import axios from "axios";
import {
  Chart as ChartJS,
  Title,
  Tooltip,
  Legend,
  LineElement,
  PointElement,
  CategoryScale,
  LinearScale,
  type ChartData,
  type ChartOptions
} from 'chart.js';
import { Line } from 'vue-chartjs';

const cidade = ref<string>("");
const latitude = ref<number | null>(null);
const longitude = ref<number | null>(null);
const resultado = ref<string | null>(null);
const erro = ref<string | null>(null);
const historico: Ref<any[]> = ref([]);

const limparModal = () => {
    resultado.value = null;
    erro.value = null;
    cidade.value = "";
    latitude.value = null;
    longitude.value = null;
    historico.value = [];
};


const consultaCoordenada = async (latitude: number | null, longitude: number | null) => {

    try {
        const { data } = await axios.post(
            "http://localhost:5063/api/consulta-clima/latlong",
            {
                latitude: latitude,
                longitude: longitude
            },
            {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem("token")}`,
                },
            },
        );

        resultado.value = data.temperatura;
        cidade.value = data.cidade;
        latitude = data.latitude;
        longitude = data.longitude;

        await consultaClimaHistorico(cidade.value, latitude!, longitude!);   
        
    } catch (error: any) {
        erro.value =
            error.response?.data?.message ||
            "Ocorreu um erro ao consultar a temperatura.";
        setTimeout(() => {
            erro.value = "";
        }, 2000);
    }
};

const consultaClimaHistorico = async (cidadeNome: string, lat: number, lon: number) => {
  try {
    const { data } = await axios.get(
      "http://localhost:5063/api/consulta-clima/consultaclima",
      {
        params: { cidade: cidadeNome, lat: lat, lon: lon },
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`
        },
      }
    );

    console.log("Histórico de consultas:", data);
    historico.value = data;
  } catch (err) {
    console.error("Erro ao buscar histórico de consultas:", err);
  }
};
ChartJS.register(Title, Tooltip, Legend, LineElement, PointElement, CategoryScale, LinearScale);

const chartData = ref<ChartData<"line">>({  
  labels: [],
  datasets: [
    {
      label: "",
      data: [],
      borderColor: "#42b983",
      backgroundColor: "rgba(66, 185, 131, 0.2)",
      fill: true,
      tension: 0.4,
    },
  ],
});

const chartOptions: ChartOptions<"line"> = reactive({
  responsive: true,
  plugins: {
    legend: { position: "top" },
  },
});

watch(historico, (novoHistorico) => {
  const ordenado = [...novoHistorico].reverse();

  chartData.value = {
    labels: ordenado.map((item) =>
      new Date(item.dataConsulta).toLocaleDateString("pt-BR")
    ),
    datasets: [
      {
        label: `Temperatura em ${cidade.value} (°C)`,
        data: ordenado.map((item) => item.temperatura),
        borderColor: "#42b983",
        backgroundColor: "rgba(66, 185, 131, 0.2)",
        fill: true,
        tension: 0.4,
      },
    ],
  };
});


</script>

<template>
    <div>
        <!-- Button trigger modal -->
        <button type="button" class="btn" data-bs-toggle="modal" data-bs-target="#exampleModal-Coordenada">
            Consulta por Coordenada
        </button>
        <!-- Modal -->
        <div class="modal fade" id="exampleModal-Coordenada" tabindex="-1" aria-labelledby="exampleModalLabel"
            aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h1 class="modal-title fs-5" id="exampleModalLabel">
                            Consulta por Coordenada
                        </h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"
                            @click="limparModal"></button>
                    </div>
                    <div class="modal-body">
                        <input type="number" class="form-control mt-3" required="true" placeholder="Digite a latitude"
                            v-model="latitude" />
                        <input type="number" class="form-control mt-3 " required placeholder="Digite a longitude"
                            v-model="longitude" />
                    </div>
                    <div v-if="resultado" >
                        <div >
                            <p class="alert alert-success m-3 justify-content">
                                A temperatura atual na das coordenadas
                                Latitude: <strong>{{ latitude }}</strong>,
                                Longitude: <strong>{{ longitude }}</strong>
                                na cidade <strong>{{ cidade }}</strong> é
                                <strong>{{ resultado }} °C</strong>
                            </p>
                        </div>
                        <Line :data="chartData" :options="chartOptions" />     
                        <table class="table">
                        <thead>
                            <tr>                                                 
                            <th scope="col-5">Cidade</th>
                            <th scope="col-1">Temperatura</th>
                            <th scope="col-1">Data</th>  
                            <th scope="col-4">Lat/Log</th>   
                            </tr>
                        </thead>
                        <tbody v-for="(item, index) in historico" :key="index">
                            <tr>                            
                            <th>{{ item.cidade }}</th>
                            <td>{{ item.temperatura }} °C</td>
                            <td>{{ new Date(item.dataConsulta).toLocaleString() }}</td>  
                            <th>{{ item.latitude }} | {{ item.longitude }}</th>
                            </tr>       
                        </tbody>
                        </table>

                    </div>
                    <div v-if="erro" class="alert alert-danger m-3 ">
                        {{ erro }}
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-success" v-if="resultado" @click="limparModal"> Nova
                            Consulta </button>
                        <button type="button" class="btn btn-primary" @click="consultaCoordenada(latitude, longitude)">
                            Consultar Temperatura
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped></style>
