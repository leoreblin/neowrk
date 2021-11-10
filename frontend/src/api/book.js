import axios from 'axios'
import Axios from 'axios'



export default {
    
    async listBooks(){        
        var user = JSON.parse(localStorage.getItem('loggedUser'));
    
        var response = await axios.get('api/books',
            {
                'headers': { 'Authorization': 'Bearer ' +  user.idToken }
            });

        return response;
    },
    async borrowBook(id) {
        var user = JSON.parse(localStorage.getItem('loggedUser'));
        const response = await axios.post(`/api/books/${id}/student/${user.email}?action=borrow`, null, 
        {
            'headers': { 'Authorization': 'Bearer ' +  user.idToken }
        });
        return response.data;
    }
}