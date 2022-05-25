class ApiClient {
    constructor(url = "https://localhost:7020/api/") {
        this.url = url;
    }

    async get(resource, options = {}) {
        return fetch(this.url + resource, {method: "GET", ...options}).then(res => flattenResponse(res));
    }

    async post(resource, data, options = {}) {
        return fetch(this.url + resource, {
            method: 'POST', body: JSON.stringify(data), ...options
        }).then(res => flattenResponse(res));
    }

    async delete(resource, data, options = {}) {
        return fetch(this.url + resource, {
            method: 'DELETE', body: JSON.stringify(data), ...options
        }).then(res => flattenResponse(res));
    }

    async put(resource, data, options = {}) {
        return fetch(this.url + resource, {method: 'PUT', body: JSON.stringify(data), ...options})
            .then(res => flattenResponse(res));
    }
}

function flattenResponse(response) {
    return response.json().then((result) => {
        console.log(result);
        return {status: response.status, statusText: response.statusText, result};
    })
}

export default ApiClient;