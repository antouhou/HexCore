pipeline {
    agent { docker { image 'mono' } }
    stages {
        stage('build') {
            steps {
                sh 'mono --version'
            }
        }
    }
}
