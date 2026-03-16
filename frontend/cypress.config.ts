import { defineConfig } from "cypress";

export default defineConfig({
  allowCypressEnv: false,

  e2e: {
    baseUrl: 'http://localhost:8080',
    specPattern: 'cypress/**/*.cy.{ts,js}',
    setupNodeEvents(on, config) {
      // implement node event listeners here
    },
  },
});

