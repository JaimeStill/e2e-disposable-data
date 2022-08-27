// import Rig from '../../rig';

describe('Brainstorm Tests', () => {
    // const rig = new Rig();
	const base = 'http://localhost:3000/';

    it('Test the Brainstorm module', async () => {
        // const connection = await rig.getConnectionString();
        cy.visit(base)
        // cy.log(`Connection string: ${connection}`)
        cy.contains('Brainstorm')
		cy.contains('Home')
        cy.screenshot('brainstorm', {
            capture: "fullPage"
        })
    })
})
