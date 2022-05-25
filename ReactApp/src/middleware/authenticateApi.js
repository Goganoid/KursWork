export function login(apiClient, email, password) {
    return apiClient.post("Authenticate/login", {email, password}, {
        headers: {
            'Content-Type': 'application/json'
        }
    });
}

export function register(apiClient, firstName, secondName, email, password) {
    return apiClient.post("Authenticate/register", {firstName, secondName, email, password}, {
        headers: {
            'Content-Type': 'application/json'
        }
    })
}