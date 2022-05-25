import { getToken } from "./storage";

const token = getToken() ?? "";


export function getAccountInfo(apiClient, id) {
    return apiClient.get(`User/account-info/${id}`);
}

export function getTendersInfo(apiClient, id) {
    return apiClient.get(`User/tenders-info/${id}`);
}

export function getCompaniesList(apiClient, id) {
    return id && apiClient.get(`User/companies-list/${id}`);
}
export function editUser(apiClient,id ,firstName, lastName,email) {
    console.log("OK")
    return apiClient.put(`user/edit/${id}`, { firstName,lastName,email }, {
        headers: {
            'Authorization': `bearer ${token}`, 'Content-Type': 'application/json'
        }
    })
}