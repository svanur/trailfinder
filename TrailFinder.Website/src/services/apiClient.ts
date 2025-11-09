import axios, {
    type AxiosInstance,
    type AxiosRequestConfig, 
    AxiosError 
} from 'axios';

class ApiClient {
    private client: AxiosInstance;

    constructor() {
        this.client = axios.create({
            baseURL: import.meta.env.VITE_API_BASE_URL,
            timeout: 30000,
            headers: {
                'Content-Type': 'application/json',
            },
        });

        // Add API key to every request
        this.client.interceptors.request.use(
            (config) => {
                const apiKey = import.meta.env.VITE_API_KEY;
                if (apiKey) {
                    config.headers['X-API-Key'] = apiKey;
                }
                return config;
            },
            (error) => {
                return Promise.reject(error);
            }
        );

        // Handle responses and errors
        this.client.interceptors.response.use(
            (response) => response,
            (error: AxiosError) => {
                if (error.response?.status === 401) {
                    console.error('API authentication failed. Check your API key.');
                } else if (error.response?.status === 429) {
                    console.error('Rate limit exceeded. Please try again later.');
                }
                return Promise.reject(error);
            }
        );
    }

    async get<T>(url: string, config?: AxiosRequestConfig): Promise<T> {
        const response = await this.client.get<T>(url, config);
        return response.data;
    }

    async post<T>(url: string, data?: unknown, config?: AxiosRequestConfig): Promise<T> {
        const response = await this.client.post<T>(url, data, config);
        return response.data;
    }

    async put<T>(url: string, data?: unknown, config?: AxiosRequestConfig): Promise<T> {
        const response = await this.client.put<T>(url, data, config);
        return response.data;
    }

    async delete<T>(url: string, config?: AxiosRequestConfig): Promise<T> {
        const response = await this.client.delete<T>(url, config);
        return response.data;
    }
}

export const apiClient = new ApiClient();