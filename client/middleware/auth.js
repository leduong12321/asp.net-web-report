export default function (context) {
    if(!context.store.getters.isAuthenticated) {
      context.redirect('/login')
    } else {
      context.redirect('/index');
    }
  }