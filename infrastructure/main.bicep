

var location = resourceGroup().location
var imageName = 'jesuscorral/concursohomebrewer:02'

var prefix = 'app-j01-'
var environment_name = 'prod'

var managed_identity_resource_suffix = 'id'
var key_vault_resource_suffix = 'kv'
var container_app_environment_resource_suffix = 'cae'
var container_app_resource_suffix = 'ca'

var managged_identity_name = '${prefix}-${environment_name}-${managed_identity_resource_suffix}'
var key_vault_name = '${prefix}-${environment_name}-${key_vault_resource_suffix}'
var container_app_environment_name = '${prefix}-${environment_name}-${container_app_environment_resource_suffix}'
var container_app_name = '${prefix}-${environment_name}-${container_app_resource_suffix}'

resource managed_identity 'Microsoft.ManagedIdentity/userAssignedIdentities@2025-01-31-preview' = {
  name: managged_identity_name
  location: location
}

resource key_vault 'Microsoft.KeyVault/vaults@2024-12-01-preview' = {
  name: key_vault_name
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    accessPolicies: [
      {
        tenantId: subscription().tenantId
        objectId: managed_identity.properties.principalId
        permissions: {
          keys: [
            'get'
            'list'
          ]
          secrets: [
            'get'
            'list'
          ]
        }
      }
    ]
  }
}

resource container_app_environment 'Microsoft.App/managedEnvironments@2025-02-02-preview' = {
  name: container_app_environment_name
  location: location
  properties: {
  }
}

var environment_variables = [
  {
    name: 'KeyVault__BaseUrl'
    value: key_vault.properties.vaultUri
  }
  {
    name: 'AZURE_CLIENT_ID'
    value: managed_identity.id
  }
]

resource container_app 'Microsoft.App/containerApps@2025-02-02-preview' = {
  name: container_app_name
  location: location
  properties: {
    environmentId: container_app_environment.id
    configuration: {
      ingress: {
        external: true
        targetPort: 8080
        allowInsecure: true
      }     
    }
    template:{
      containers: [
        {
          name: 'app-container'
          image: imageName
          resources: {
            cpu: 2
            memory: '4Gi'
          }
          env: environment_variables
        }
      ]
      scale: {
        minReplicas: 1
        maxReplicas: 1
      }
    }
    
  }
  identity: {
      type: 'UserAssigned'
      userAssignedIdentities: {
        '${managed_identity.id}': {}
      }
    }
}
