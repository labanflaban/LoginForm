// Function to retrieve the JWT token from localStorage
function getToken(){
    return localStorage.getItem('token');
  }
  
  // Function to make authenticated requests
  function makeAuthenticatedRequest(url, method, data = null) {
    const token = getToken();
  
    if(!token){
      console.error('No token found. User may not be authenticated.');
      return;
    }
  
    const headers = {
      'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
    };
  
    const requestOptions = {
      method: method,
      headers: headers
    };
  
    if(data){
      requestOptions.body = JSON.stringify(data);
    }
  
    return fetch(url, requestOptions)
      .then(response => {
        if (!response.ok){
          throw new Error('Network response was not ok');
        }
        return response.json();
      })
      .catch(error => {
        console.error('Error', error);
      })
  }
  
  
  document.getElementById('createAccountForm').addEventListener('submit', function(event) {
      event.preventDefault(); // Prevent the default form submission
      
      // Get the form data 
      const formData = new FormData(event.target);
      
      // Create an object from form data
      const data = {};
      formData.forEach((value, key) => {
          data[key] = value;
      });
      
      
      // An HTTP POST request to the API server
      fetch('https://localhost:7025/api/Users', {
          method: 'POST',
          headers: {
              'Content-Type': 'application/json',
          },
          body: JSON.stringify(data),
      })
      .then(response => {
          if (!response.ok) {
              throw new Error('Network response was not ok');
          }
          return response.json();
      })
      .then(data => {
          console.log(data);
      })
      .catch(error => {
          console.error('Error:', error);
      });
  });
  
  document.getElementById('loginForm').addEventListener('submit', function(event){
    event.preventDefault();
  
    //Get the form data
    const formData = new FormData(event.target);
  
    const data = {};
    formData.forEach((value, key) => {
      data[key] = value;
    });
  
    fetch('https://localhost:7025/api/Login', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(data),
    })
    .then(response => {
      if(!response.ok) {
        throw new Error('Network response was not ok');
      }
      return response.json();
    })
    .then(data => {
      //handle the authentication response 
      if(data.success){
        console.log('Login successfull');
  
        //Save the token to local storage
        localStorage.setItem('token', data.token); // Assuming the token is returned in the response
      } else {
        console.error('Login failed', data.error);
      }
    })
    .catch(error => {
      console.error('Error', error);
    })
  });
  