const PAGE_ROUTES: any = {
  
  DEFAULT: {
    label: "Splah Page",
    path: "/",
  },
  
  SIGN_IN: {
    label: "Sign In",
    path: "/sign-in",
  },
  
  SIGN_UP: {
    label: "Sign Up",
    path: "/sign-up",
  },
  
  ADMIN: {
    label: "Administration",
    path: "/admin",
    
    DASHBOARD: {
      label: "Dashboard",
      path: "/admin/dashboard",
    },
    
    AUTHORS: {
      label: "Authors",
      path: "/admin/authors",
      endpoint: "authors",

      CREATE: {
        label: "Create Author",
        path: "/admin/authors/create",
        endpoint: "authors",
      },

      EDIT: {
        label: "Edit Author",
        path: "/admin/authors/:id",
        endpoint: "authors/:id",
      },
    },
    
    BOOKS: {
      label: "Books",
      path: "/admin/books",
      endpoint: "books",

      CREATE: {
        label: "Create Books",
        path: "/admin/books/create",
        endpoint: "books",
      },

      EDIT: {
        label: "Edit Book",
        path: "/admin/books/:id",
        endpoint: "books/:id",
      },
    },
    
    ARTISTS: {
      label: "Artists",
      path: "/admin/artists",
      endpoint: "artists",

      CREATE: {
        label: "Create Artist",
        path: "/admin/artists/create",
        endpoint: "artists",
      },

      EDIT: {
        label: "Edit Artist",
        path: "/admin/artists/:id",
        endpoint: "artists/:id",
      },
    },
    
    EMAIL_SUBSCRIPTIONS: {
      label: "Email Subscriptions",
      path: "/admin/email-subscrptions",
      endpoint: "email-subscriptions",

      CREATE: {
        label: "Create Email Subscription",
        path: "/admin/email-subscriptions/create",
        endpoint: "email-subscriptions",
      },

      EDIT: {
        label: "Edit Email Subscription",
        path: "/admin/email-subscriptions/:id",
        endpoint: "email-subscriptions/:id",
      },
    },
    
    USERS: {
      label: "Users",
      path: "/admin/users",
      endpoint: "users",

      CREATE: {
        label: "Create User",
        path: "/admin/users/create",
        endpoint: "users",
      },

      EDIT: {
        label: "Edit User",
        path: "/admin/users/:id",
        endpoint: "users/:id",
      },
    },
    
    RECYCLE_BIN: {
      label: "Recycle Bin",
      path: "/admin/recycle-bin",
      endpoint: "recycle-bin"
    },
  },
};

export default PAGE_ROUTES;
