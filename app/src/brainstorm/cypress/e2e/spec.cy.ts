describe('Brainstorm Tests', () => {
	const base = 'http://localhost:3000/'

    it('Test the Brainstorm module', () => {
        cy.visit(base)
        cy.contains('Brainstorm')
		cy.contains('Home')
        cy.screenshot('brainstorm', {
            capture: "fullPage"
        })
    })
})
