export default function (context) {
    if(!context.store.getters.user?.Id) context.redirect('/login');
  }