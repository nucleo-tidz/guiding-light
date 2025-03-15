
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
