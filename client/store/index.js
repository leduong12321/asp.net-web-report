import Vuex from 'vuex'
const createStore = () => {
    return new Vuex.Store({
        state: {
            user: {},
            isToast: false,
            token: null,
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
                vuexContext.commit('setToken', token)
            },
            setLogoutTimer(vuexContext, duration) {
                setTimeout(() => {
                    vuexContext.commit('clearToken')
                }, duration)
            },
            initAuth(vuexContext) {
                const token = localStorage.getItem('token');
                const tokenExpiration = localStorage.getItem('tokenExpiration');
                if(new Date().getTime() > tokenExpiration || !token) return false;
                vuexContext.commit('setToken', token);
            }
        },
        getters: {
            user(state) {
                return state.user
            },
            isAuthenticated(state) {
                console.log('auth', state.token);
                return state.token != null;
            },
            isToast(state) {
                return state.isToast
            },
            getToken(state) {
                console.log('get token', state.token.data.data);
                return state.token.data.data
            }
        },
    })
}

export default createStore