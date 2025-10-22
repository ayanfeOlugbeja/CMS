# CMS Employee Management API Documentation

## Overview
This API provides comprehensive employee management functionality with role-based access control, authentication, and organizational structure management.

## Base URL
- Development: `https://localhost:7176` or `http://localhost:5249`
- Swagger UI: `https://localhost:7176` (root path)

## Authentication
The API uses JWT Bearer token authentication. Include the token in the Authorization header:
```
Authorization: Bearer {your-jwt-token}
```

## Standard Response Format
All API responses follow this consistent format:

### Success Response
```json
{
  "success": true,
  "message": "Operation successful",
  "data": { /* response data */ },
  "timestamp": "2024-01-01T00:00:00.000Z"
}
```

### Error Response
```json
{
  "success": false,
  "message": "Error description",
  "errors": ["Validation error 1", "Validation error 2"],
  "timestamp": "2024-01-01T00:00:00.000Z"
}
```

## API Endpoints

### Authentication
- `POST /api/Authentication/register` - User registration
- `POST /api/Authentication/login` - User login
- `POST /api/Authentication/verify-otp` - Email verification
- `POST /api/Authentication/forgot-password` - Password reset request
- `POST /api/Authentication/reset-password` - Password reset

### Employee Management
- `GET /api/Employees` - Get all employees (Admin only)
- `GET /api/Employees/{id}` - Get employee by ID
- `POST /api/Employees` - Create employee (Admin only)
- `PUT /api/Employees/{id}` - Update employee (Admin only)
- `PATCH /api/Employees/{id}/deactivate` - Deactivate employee (Admin only)

### Organizational Structure
- `GET /api/Departments` - Get all departments
- `POST /api/Departments` - Create department
- `PUT /api/Departments/{id}` - Update department
- `DELETE /api/Departments/{id}` - Delete department

- `GET /api/Branches` - Get all branches
- `POST /api/Branches` - Create branch
- `PUT /api/Branches/{id}` - Update branch
- `DELETE /api/Branches/{id}` - Delete branch

- `GET /api/Towns` - Get all towns
- `POST /api/Towns` - Create town
- `PUT /api/Towns/{id}` - Update town
- `DELETE /api/Towns/{id}` - Delete town

### Role & Permission Management
- `GET /api/Roles` - Get all roles
- `POST /api/Roles` - Create role
- `PUT /api/Roles/{id}` - Update role
- `DELETE /api/Roles/{id}` - Delete role

### Job Management
- `GET /api/Jobs` - Get all jobs
- `POST /api/Jobs` - Create job
- `PUT /api/Jobs/{id}` - Update job
- `DELETE /api/Jobs/{id}` - Delete job

## Error Codes
- `400 Bad Request` - Invalid input data or validation errors
- `401 Unauthorized` - Missing or invalid authentication token
- `403 Forbidden` - Insufficient permissions
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Server error

## CORS Configuration
The API is configured to accept requests from:
- `http://localhost:3000` (React)
- `http://localhost:4200` (Angular)
- `http://localhost:5173` (Vite)
- HTTPS versions of the above

## Frontend Integration Examples

### React/JavaScript Example
```javascript
// Login
const login = async (email, password) => {
  const response = await fetch('/api/Authentication/login', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ email, password })
  });
  
  const result = await response.json();
  if (result.success) {
    localStorage.setItem('token', result.token);
  }
  return result;
};

// Get Employees (with authentication)
const getEmployees = async () => {
  const token = localStorage.getItem('token');
  const response = await fetch('/api/Employees', {
    headers: {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    }
  });
  
  return await response.json();
};
```

### Angular/TypeScript Example
```typescript
// Service
@Injectable()
export class EmployeeService {
  private baseUrl = 'https://localhost:7176/api';
  
  getEmployees(): Observable<ApiResponse<EmployeeDto[]>> {
    return this.http.get<ApiResponse<EmployeeDto[]>>(`${this.baseUrl}/Employees`);
  }
  
  createEmployee(employee: CreateEmployeeDto): Observable<ApiResponse<EmployeeDto>> {
    return this.http.post<ApiResponse<EmployeeDto>>(`${this.baseUrl}/Employees`, employee);
  }
}
```

## Validation Rules

### Employee Creation
- First Name: Required, 2-50 characters
- Last Name: Required, 2-50 characters
- Email: Required, valid email format
- Telephone: Required, 9-digit number
- Department ID: Required, positive number
- Branch ID: Required, positive number
- Town ID: Required, positive number

### Department Creation
- Department Name: Required, 2-100 characters

## Rate Limiting
Currently no rate limiting is implemented, but it's recommended for production use.

## Security Features
- JWT token authentication
- Role-based authorization
- CORS protection
- Input validation
- SQL injection protection (Entity Framework)
- Password hashing (BCrypt)
