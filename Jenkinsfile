import org.jenkinsci.plugins.pipeline.modeldefinition.Utils

node {
	try {
		properties([
			parameters([
				credentials(credentialType: 'com.browserstack.automate.ci.jenkins.BrowserStackCredentials', defaultValue: '', description: 'Select your BrowserStack Username', name: 'BROWSERSTACK_USERNAME', required: true),
				[$class: 'ExtensibleChoiceParameterDefinition',
				choiceListProvider: [
					$class: 'TextareaChoiceListProvider',
					addEditedValue: false,
					choiceListText: '''Single
Parallel
Local
Mobile
Local_Parallel''',
					defaultChoice: 'Single'
				],
				description: 'Select the test you would like to run',
				editable: false,
				name: 'TEST_TYPE']
			])
		])

		stage('Pull from Github') {
			dir('test') {
				git branch: 'develop', changelog: false, poll: false, url: 'https://github.com/mayankmaurya21/browserstack-examples-specflowplus-1.git'
			}
		}

		stage('Start Local') {
			if ( TEST_TYPE == 'Local') {
				dir('app') {
					git branch: 'master', url: 'https://github.com/browserstack/browserstack-demo-app'
					sh '''
npm install
npm run build
npm start &
					'''
				}
			} else {
				Utils.markStageSkippedForConditional('Start Local')
			}
		}

		stage('Install Dependencies'){
		    echo "workspace directory is ${workspace}"
		   sh '''cd test
dotnet restore'''
		}

		stage('Run Test(s)') {
			browserstack(credentialsId: "${params.BROWSERSTACK_USERNAME}") {
			    if (TEST_TYPE == 'Single') {
				sh '''cd test
dotnet test --filter "TestCategory= Single" '''
			}
			 else if (TEST_TYPE == 'Parallel'){
			     	sh '''cd test
dotnet test --filter "TestCategory= Parallel" '''
			 }
			 else if (TEST_TYPE == 'Local'){
			     	sh '''cd test
dotnet test --filter "TestCategory= Local" '''
			 }
			 else if (TEST_TYPE == 'Mobile'){
			     	sh '''cd test
dotnet test --filter "TestCategory= Mobile" '''
			 }
			 else if (TEST_TYPE == 'Local_Parallel'){
			     	sh '''cd test
dotnet test --filter "TestCategory= Local_Parallel" '''
			 }
			}
		}
	} catch (e) {
		currentBuild.result = 'FAILURE'
		echo e
	} finally {
		stage('Publish Results'){
			echo "Test execution Completed !!"
			browserStackReportPublisher 'automate'
		}
	}
}
