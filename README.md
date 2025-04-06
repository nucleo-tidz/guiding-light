
## Overview  
Guiding Light AI is a faith-based AI assistant designed to provide spiritual guidance, scriptural insights, and wisdom from religious texts. Built with **C# Semantic Kernel**, **Kernel Memory**, and **RAG (Retrieval-Augmented Generation)**, this AI pastor offers personalized and denomination-specific responses, making it adaptable to multiple faith traditions.  

## Features  
- **Dynamic RAG-Based Responses** – Fetches relevant religious texts dynamically.  
- **Denomination Customization** – Configurable to follow specific theological perspectives.  
- **Secure and Private** – One user per session to ensure personalized interactions.  
- **Scalable Multi-Faith Support** – Designed to extend beyond Christianity to other faiths.  
- **Azure-Powered** – Uses Azure Storage, Redis, and Semantic Kernel for optimal performance.  

## Architecture  
- **.NET 8 & C#** – Core framework for development.  
- **Semantic Kernel & Kernel Memory** – AI-driven text generation and vector storage.  
- **Redis Memory Store** – Efficient retrieval of religious texts and past interactions.  
- **Azure Blob Storage** – Stores theological documents for reference.  

## Getting Started  
### Prerequisites  
- .NET 8 SDK installed  
- Azure Storage and Redis setup  
- API key for Semantic Kernel integration  

### Installation  
1. Clone the repository:  
   ```sh
   git clone https://github.com/your-org/guiding-light-ai.git
   cd guiding-light-ai
### Installation  
1. Setup AKS
2. Download kube config
    ```sh
   az login
   az aks get-credentials --name <name of aks service> --resource-group <rg of aks service>
3. Install Ingress controller
   ```sh
   helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
   helm repo update
   helm install ingress-nginx ingress-nginx/ingress-nginx --namespace ingress-nginx --create-namespace --set controller.service.externalTrafficPolicy=Local
4. Map DNS Mapping
   ```sh
   az network public-ip list
   az network public-ip update <id reterived from above step if there are multiple id use  nginx one see secreenshot below > -dns-name "<dns name of your choice e.g guidinglight>" ```
![image](https://github.com/user-attachments/assets/5a43a16d-adad-4a1f-aa05-289bcdd891a5)


   
5. Deploy , Access the app  ```<dnsname>.<azuregion>.cloudapp.azure.com``` e.g ahmar.centralindia.cloudapp.azure.com/
6. Verify ingress external ip address is same with fqnd ip adress
   ```kubectl get svc -n ingress-nginx```
   ### 🌐 Ingress Controller Service

| NAME                     | TYPE         | CLUSTER-IP   | EXTERNAL-IP     | PORT(S)                    |
|--------------------------|--------------|--------------|------------------|-----------------------------|
| ingress-nginx-controller| LoadBalancer | 10.0.0.15    | 20.204.56.123    | 80:80/TCP,443:443/TCP      |

```nslookup <dnsname>.<azuregion>.cloudapp.azure.com```
### 🌐 Public IP and DNS Mapping

| IP Address       | DNS Name                                           |
|------------------|----------------------------------------------------|
| 20.204.56.123    | ```<dnsname>.<azuregion>.cloudapp.azure.com```     |



   
 
