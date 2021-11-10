<template>
  <v-container>
    <div class="home">
      <v-data-table
        :headers="headers"
        :items.sync="books"
        hide-default-footer
        class="elevation-1"
      >
        <template v-slot:item="row">
          <tr>
            <td>{{ row.item.title }}</td>
            <td>{{ row.item.author }}</td>
            <td>{{ row.item.pages }}</td>
            <td>{{ row.item.publisher }}</td>
            <td>
              <v-btn @click="borrowBook(row.item.id)">Pegar</v-btn>
            </td>
          </tr>
        </template>
      </v-data-table>

      <v-snackbar v-model="snackbar">
        {{ errorMessage }}

        <template v-slot:action="{ attrs }">
          <v-btn v-bind="attrs" @click="snackbar = false"> Close </v-btn>
        </template>
      </v-snackbar>
    </div>
    <Loading :active.sync="loading" :is-full-page="true"></Loading>
  </v-container>
</template>

<script>
import bookApi from "@/api/book";
import Loading from "vue-loading-overlay";
import "vue-loading-overlay/dist/vue-loading.css";

export default {
  name: "Home",

  data() {
    return {
      loading: false,
      snackbar: false,
      errorMessage: "",
      headers: [
        {
          text: "Titulo",
          align: "start",
          sortable: false,
          value: "title",
        },
        { text: "Autor", value: "author" },
        { text: "Nº Páginas", value: "pages" },
        { text: "Editora", value: "editor" },
        { text: "", value: "" },
      ],
      books: [],
    };
  },
  async mounted() {
    try {
      this.loading = true;
      await this.listBooks();
    } catch (err) {
      console.log(err);
      this.errorMessage = err.message;
      this.snackbar = true;
      if(err.message.includes('401')){
        this.$router.push('/login');
      }
    } finally {
      this.loading = false;
    }
  },
  methods: {
    async borrowBook(id) {
      try {
        this.loading = true;
        const responseData = await bookApi.borrowBook(id);
        console.log(responseData);
        if(responseData.success == false) {
          responseData.messages.forEach(element => {
            this.errorMessage = element;
            this.snackbar = true;           
          });
        } else {
          this.errorMessage = responseData.messages[0];
          this.snackbar = true;
        }
        await this.listBooks();
      } catch (err) {
        console.log(err);
        this.errorMessage = err.message;
        this.snackbar = true;
        if(err.message.includes('401')) {
          this.$router.push('/login');
        }
      } finally {
        this.loading = false;
      }
    },
    async listBooks() {
      const allBooks = await bookApi.listBooks();
      this.books = allBooks.data;
    },
  },
  components: {
    Loading,
  },
};
</script>

<style scoped>
.home {
  padding: 30px;
}
</style>
