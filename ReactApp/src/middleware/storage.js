export function setUserData({token, userId}) {
    console.log(token, userId);
    localStorage.setItem('token', token);
    localStorage.setItem('userId', userId);
}

export function isLoggedIn() {
    return localStorage.getItem('token') != null;
}

export function getToken() {
    return localStorage.getItem('token');
}

export function getUserId() {
    return parseInt(localStorage.getItem('userId'));
}