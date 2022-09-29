import Vuex from 'vuex'
const createStore = () => {
    return new Vuex.Store({
        state: {
            user: {},
            isToast: false
        },
        mutations: {
            setUser(state, user) {
                state.user = user;
            },
            setToast(state, isToast) {
                state.isToast = isToast;
            }
        },
        actions: {
            setUser(vuexContext, user) {
                vuexContext.commit('setUser', user)
            },
            setToast(vuexContext, isToast) {
                vuexContext.commit('setToast', isToast)
            }
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
        },
    })
}

export default createStore