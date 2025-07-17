// src/types/api.ts
export interface ApiError {
    message: string;
    statusCode: number;
}

export interface ApiResponse<T> {
    data: T;
    message?: string;
}

export interface PaginatedResponse<T> {
    items: T[];
    totalCount: number;
    pageNumber: number;
    pageSize: number;
    totalPages: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
}

export interface TrailNotFoundResponse {
    message: string;
    suggestions?: { id: string; name: string; slug: string; }[];
}

// Define a custom error type to carry suggestions
export class TrailNotFoundError extends Error {
    suggestions: { id: string; name: string; slug: string; }[];
    statusCode: number;

    constructor(message: string, suggestions: { id: string; name: string; slug: string; }[] = [], statusCode: number = 404) {
        super(message);
        this.name = 'TrailNotFoundError';
        this.suggestions = suggestions;
        this.statusCode = statusCode;
        // Set the prototype explicitly.
        Object.setPrototypeOf(this, TrailNotFoundError.prototype);
    }
}