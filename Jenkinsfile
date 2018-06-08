pipeline {

    agent { docker { image 'mono' } }

    environment {
        HOME = '/home'
    }

    stages {
        stage('Install Dependencies') {
            steps {
		sh 'mkdir .config'
                sh 'nuget restore'
            }
        }
        stage('Build') {
            steps {
                sh 'msbuild'
            }
        }
        stage('Test') {
            steps {
                sh 'echo "Tests should be there"'
            }
        }
    }
}
