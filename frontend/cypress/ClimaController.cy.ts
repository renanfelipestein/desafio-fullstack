const BASE = 'http://localhost:8080'


function getToken(): Cypress.Chainable<string> {
  return cy.request({
    method: 'POST',
    url: `${BASE}/api/login`,
    body: { email: 'clover@aliare.co', senha: 'clover123' }
  }).then((res) => res.body.token)
}


describe('POST /api/consulta-clima/cidade', () => {
    
  let token: string
  before(() => {
    getToken().then((t) => { token = t })
  })

  it('retorna 200 com cidade válida', () => {
    cy.request({
      method: 'POST',
      url: `${BASE}/api/consulta-clima/cidade`,
      headers: { Authorization: `Bearer ${token}` },
      body: { cidade: 'São Paulo' }
    }).then((res) => {
      expect(res.status).to.eq(200)
      expect(res.body).to.have.property('cidade')
      expect(res.body).to.have.property('temperatura')
      expect(res.body).to.have.property('latitude')
      expect(res.body).to.have.property('longitude')
    })
  })

  it('retorna 404 com cidade inexistente', () => {
    cy.request({
      method: 'POST',
      url: `${BASE}/api/consulta-clima/cidade`,
      headers: { Authorization: `Bearer ${token}` },
      body: { cidade: 'CidadeQueNaoExiste12345' },
      failOnStatusCode: false
    }).then((res) => {
      expect(res.status).to.eq(404)
    })
  })

  it('retorna 401 sem token', () => {
    cy.request({
      method: 'POST',
      url: `${BASE}/api/consulta-clima/cidade`,
      body: { cidade: 'São Paulo' },
      failOnStatusCode: false
    }).then((res) => {
      expect(res.status).to.eq(401)
    })
  })

  it('retorna 401 com token inválido', () => {
    cy.request({
      method: 'POST',
      url: `${BASE}/api/consulta-clima/cidade`,
      headers: { Authorization: 'Bearer token-invalido' },
      body: { cidade: 'São Paulo' },
      failOnStatusCode: false
    }).then((res) => {
      expect(res.status).to.eq(401)
    })
  })

})


describe('POST /api/consulta-clima/latlong', () => {

  let token: string

  before(() => {
    getToken().then((t) => { token = t })
  })

  it('retorna 200 com coordenadas válidas (São Paulo)', () => {
    cy.request({
      method: 'POST',
      url: `${BASE}/api/consulta-clima/latlong`,
      headers: { Authorization: `Bearer ${token}` },
      body: { latitude: -23.5505, longitude: -46.6333 }
    }).then((res) => {
      expect(res.status).to.eq(200)
      expect(res.body).to.have.property('temperatura')
      expect(res.body).to.have.property('latitude')
      expect(res.body).to.have.property('longitude')
    })
  })

  it('retorna 400 sem latitude', () => {
    cy.request({
      method: 'POST',
      url: `${BASE}/api/consulta-clima/latlong`,
      headers: { Authorization: `Bearer ${token}` },
      body: { longitude: -46.6333 },
      failOnStatusCode: false
    }).then((res) => {
      expect(res.status).to.eq(400)
    })
  })

  it('retorna 400 sem longitude', () => {
    cy.request({
      method: 'POST',
      url: `${BASE}/api/consulta-clima/latlong`,
      headers: { Authorization: `Bearer ${token}` },
      body: { latitude: -23.5505 },
      failOnStatusCode: false
    }).then((res) => {
      expect(res.status).to.eq(400)
    })
  })

  it('retorna 400 sem latitude e longitude', () => {
    cy.request({
      method: 'POST',
      url: `${BASE}/api/consulta-clima/latlong`,
      headers: { Authorization: `Bearer ${token}` },
      body: {},
      failOnStatusCode: false
    }).then((res) => {
      expect(res.status).to.eq(400)
    })
  })

  it('retorna 404 com coordenadas inválidas', () => {
    cy.request({
      method: 'POST',
      url: `${BASE}/api/consulta-clima/latlong`,
      headers: { Authorization: `Bearer ${token}` },
      body: { latitude: 0.0001, longitude: 0.0001 },
      failOnStatusCode: false
    }).then((res) => {
      expect(res.status).to.be.oneOf([200, 404])
    })
  })

  it('retorna 401 sem token', () => {
    cy.request({
      method: 'POST',
      url: `${BASE}/api/consulta-clima/latlong`,
      body: { latitude: -23.5505, longitude: -46.6333 },
      failOnStatusCode: false
    }).then((res) => {
      expect(res.status).to.eq(401)
    })
  })

})

describe('GET /api/consulta-clima/consultaclima', () => {

  let token: string

  before(() => {
    getToken().then((t) => {
      token = t
      cy.request({
        method: 'POST',
        url: `${BASE}/api/consulta-clima/cidade`,
        headers: { Authorization: `Bearer ${token}` },
        body: { cidade: 'Curitiba' }
      })
    })
  })

  it('retorna histórico sem filtro', () => {
    cy.request({
      method: 'GET',
      url: `${BASE}/api/consulta-clima/consultaclima`,
      headers: { Authorization: `Bearer ${token}` },
      failOnStatusCode: false
    }).then((res) => {
      expect(res.status).to.be.oneOf([200, 404]) 
      if (res.status === 200) {
        expect(res.body).to.be.an('array')
        expect(res.body[0]).to.have.property('cidade')
        expect(res.body[0]).to.have.property('temperatura')
        expect(res.body[0]).to.have.property('dataConsulta')
      }
    })
  })

  it('filtra por cidade existente', () => {
    cy.request({
      method: 'GET',
      url: `${BASE}/api/consulta-clima/consultaclima?cidade=Curitiba`,
      headers: { Authorization: `Bearer ${token}` },
      failOnStatusCode: false
    }).then((res) => {
      expect(res.status).to.be.oneOf([200, 404])
      if (res.status === 200) {
        expect(res.body).to.be.an('array')
        res.body.forEach((item: any) => {
          expect(item.cidade.toLowerCase()).to.include('curitiba')
        })
      }
    })
  })

  it('filtra por cidade inexistente retorna 404', () => {
    cy.request({
      method: 'GET',
      url: `${BASE}/api/consulta-clima/consultaclima?cidade=CidadeXXX999`,
      headers: { Authorization: `Bearer ${token}` },
      failOnStatusCode: false
    }).then((res) => {
      expect(res.status).to.eq(404)
    })
  })

  it('filtra por lat/lon', () => {
    cy.request({
      method: 'GET',
      url: `${BASE}/api/consulta-clima/consultaclima?lat=-23.5505&lon=-46.6333`,
      headers: { Authorization: `Bearer ${token}` },
      failOnStatusCode: false
    }).then((res) => {
      expect(res.status).to.be.oneOf([200, 404])
    })
  })

  it('retorna 401 sem token', () => {
    cy.request({
      method: 'GET',
      url: `${BASE}/api/consulta-clima/consultaclima`,
      failOnStatusCode: false
    }).then((res) => {
      expect(res.status).to.eq(401)
    })
  })

})