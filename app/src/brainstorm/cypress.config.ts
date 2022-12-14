import { defineConfig } from 'cypress';

export default defineConfig({
    e2e: {
        baseUrl: 'http://localhost:3000',
        reporter: '../../node_modules/mochawesome',
        defaultCommandTimeout: 8000,
        reporterOptions: {
            reportDir: 'cypress/results',
            reportFilename: '[datetime]_[status]',
            timestamp: 'yyyy-mm-dd-HH-MM',
            overwrite: false,
            html: false,
            json: true
        }
    }
})
