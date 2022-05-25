import { getToken } from "./storage";

const token = getToken() ?? "";

export function getCount(apiClient) {
    return apiClient.get("tender/count");
}

export function getTenders(apiClient, n, searchTerm = "") {
    console.log("start getting");
    return apiClient.get(`tender/tenders/${n}${searchTerm !== '' ? '/?searchTerm=' + searchTerm : ''}`);
}

export function getTender(apiClient, id) {
    return apiClient.get(`tender/${id}`);
}

export function postTender(apiClient, companyId, endDate, title, cost) {
    return apiClient.post("tender/create", { title, cost, endDate, companyId }, {
        headers: {
            'Authorization': `bearer ${token}`, 'Content-Type': 'application/json'
        }
    })
}


export function toggleTender(apiClient, id) {
    return apiClient.put(`tender/toggle/${id}`, {}, {
        headers: {
            'Authorization': `bearer ${token}`,
        }
    })
}

export function removeTender(apiClient, id) {
    return apiClient.delete(`tender/remove/${id}`, {}, {
        headers: {
            'Authorization': `bearer ${token}`,
        }
    })
}

export function unsubscribe(apiClient, id) {
    return apiClient.delete(`tender/unsubscribe/${id}`, {}, {
        headers: {
            'Authorization': `bearer ${token}`,
        }
    })
}

export function subscribeToTender(apiClient, companyId, tenderId, cost) {
    return apiClient.put(`tender/subscribe/${companyId}/${tenderId}/${cost}`, {}, {
        headers: {
            'Authorization': `bearer ${token}`,
        }
    })
}

export function setExecutor(apiClient, tenderId, executorId) {
    return apiClient.put(`tender/set-executor/${tenderId}/${executorId}`, {}, {
        headers: {
            'Authorization': `bearer ${token}`,
        }
    })
}

