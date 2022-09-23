import Vuex from 'vuex'
const createStore = () => {
    return new Vuex.Store({
        state: {
            user: {}
        },
        mutations: {
            setUser(state, user) {
                state.user = user;
            }
        },
        actions: {
            setUser(vuexContext, user) {
                vuexContext.commit('setUser', user)
            }
        },
        getters: {
            user(state) {
                return state.user
            },
            isAuthenticated(state) {
                // return state.token != null;
                state.user != null;
            }
        },
    })
}

export default createStore