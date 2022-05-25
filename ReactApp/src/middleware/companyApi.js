import { getToken } from "./storage";

const token = getToken() ?? "";

export function getCompanyTendersInfo(apiClient, id) {
    return apiClient.get(`Company/tenders-info/${id ?? ''}`);
}

export function getCompanyInfo(apiClient, id) {
    return apiClient.get(`Company/info/${id ?? ''}`);
}

export function removeCompany(apiClient, id) {
    return apiClient.delete(`company/remove/${id}`, {}, {
        headers: {
            'Authorization': `bearer ${token}`,
        }
    })
}

export function addCompany(apiClient, name, location) {
    return apiClient.post("company/add", { name, location }, {
        headers: {
            'Authorization': `bearer ${token}`, 'Content-Type': 'application/json'
        }
    })
}
export function editCompany(apiClient,id ,name, location) {
    return apiClient.put(`company/edit/${id}`, { name, location }, {
        headers: {
            'Authorization': `bearer ${token}`, 'Content-Type': 'application/json'
        }
    })
}