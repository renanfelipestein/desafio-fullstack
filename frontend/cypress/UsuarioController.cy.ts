describe('API - Login', () => {

  it('POST /api/login retorna 200 e token', () => {
    cy.request({
      method: 'POST',
      url: '/api/login',
      body: { email: 'clover@aliare.co', senha: 'clover123' }
    }).then((res) => {
      expect(res.status).to.eq(200)
      expect(res.body).to.have.property('token')
    })
  })

  it('POST /api/login com senha errada retorna 401', () => {
    cy.request({
      method: 'POST',
      url: '/api/login',
      body: { email: 'clover@aliare.co', senha: '123' },
      failOnStatusCode: false
    }).then((res) => {
      expect(res.status).to.eq(401)
    })
  })

})         