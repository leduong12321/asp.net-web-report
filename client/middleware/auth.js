export default function (context) {
  // if(!context.store.getters.user?.Id) context.redirect('/login');
  if(!localStorage.getItem('token')) context.redirect('/login');
}

  