import Vuex from 'vuex'
const createStore = () => {
    return new Vuex.Store({
        state: {
            user: {},
            isToast: false,
            token: null
        },
        mutations: {
            setUser(state, user) {
                state.user = user;
            },
            setToast(state, isToast) {
                state.isToast = isToast;
            },
            setToken(state, token) {
                state.token = token;
            },
            clearToken(state) {
                state.token = null;
            },
        },
        actions: {
            setUser(vuexContext, user) {
                vuexContext.commit('setUser', user)
            },
            setToast(vuexContext, isToast) {
                vuexContext.commit('setToast', isToast)
            },
            authenticateUser(vuexContext, token) {
                if(!localStorage.getItem('refreshToken')) {
                    let timeGetToken = new Date().valueOf();
                    let expiresRefresh = timeGetToken + 7200000;
                    localStorage.setItem('refreshToken', expiresRefresh);
                }
                vuexContext.commit('setToken', token)
            },
            refreshToken() {
                if(localStorage.getItem('refreshToken') < new Date().valueOf()) {
                    localStorage.clear();
                }
            },
            clearToken() {
                localStorage.clear()
            },
        },
        getters: {
            user(state) {
                return state.user
            },
            isAuthenticated(state) {
                // return state.token != null;
                state.user != null;
            },
            isToast(state) {
                return state.isToast
            },
            getToken(state) {
                return state.token.data.data
            }
        },
    })
}

export default createStore