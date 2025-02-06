import axios from 'axios';

const apiUrl = "http://localhost:5285"
export default {
  getTasks: async () => {
    console.log('getTasks')
    const result = await axios.get(`${apiUrl}/items`)
    return result.data
  },

  addTask: async (name) => {
    console.log('addTask', name)
    const result = await axios.post(`${apiUrl}/items`, { name })
    return result.data
  },

  setCompleted: async (id) => {
    console.log('setCompleted', { id })
    const result = await axios.put(`${apiUrl}/items/${id}`)
    return result.data
  },

 
  deleteTask: async (id) => {
    console.log('deleteTask', id)
    const result = await axios.delete(`${apiUrl}/items/${id}`)
    return result.data
  }
};
// import axios from 'axios';
// י
// // הגדרת כתובת ה-API כ-default
// const apiUrl = "http://localhost:5215";
// const apiClient = axios.create({
//     baseURL: apiUrl,
// });

// // הוספת interceptor לתפיסת שגיאות
// apiClient.interceptors.response.use(
//     response => {
//         // אם התגובה מצליחה, מחזירים את התגובה
//         return response;
//     },
//     error => {
//         // כאן תופסים את השגיאות
//         console.error('Error response:', error.response);
//         // ניתן להוסיף לוגיקה נוספת כמו רישום ללוגים או התראות
//         return Promise.reject(error); // מחזירים את השגיאה הלאה
//     }
// );

// export default {
//   getTasks: async () => {
//     console.log('getTasks');
//     const result = await apiClient.get('/items'); // השתמש ב-client המוגדר
//     return result.data;
//   },

//   addTask: async (name) => {
//     console.log('addTask', name);
//     const result = await apiClient.post('/items', { name }); // השתמש ב-client המוגדר
//     return result.data;
//   },

//   setCompleted: async (id, isComplete) => {
//     console.log('setCompleted', { id, isComplete });
//     const result = await apiClient.put(`/items/${id}`, { isComplete }); // השתמש ב-client המוגדר
//     return result.data;
//   },

//   deleteTask: async (id) => {
//     console.log('deleteTask', id);
//     const result = await apiClient.delete(`/items/${id}`); // השתמש ב-client המוגדר
//     return result.data;
//   }
// };