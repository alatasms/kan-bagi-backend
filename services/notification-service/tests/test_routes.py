import unittest
from unittest.mock import patch, MagicMock
from flask import Flask
from flask_restx import Api
from app.routes.email_routes import email_ns
from app.services import email_service
from mongoengine import connect, disconnect
import mongomock
import json

class TestRoutes(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        # Test için Flask uygulaması oluştur
        cls.app = Flask(__name__)
        cls.app.config['TESTING'] = True
        
        # API oluştur ve namespace'leri kaydet
        cls.api = Api(cls.app)
        cls.api.add_namespace(email_ns, path='/api/email')
        
        # Test client oluştur
        cls.client = cls.app.test_client()
        
        # Test için geçici MongoDB bağlantısı
        disconnect()
        connect('mongoenginetest', mongo_client_class=mongomock.MongoClient)

    def setUp(self):
        # Her test öncesi veritabanını temizle
        disconnect()
        connect('mongoenginetest', mongo_client_class=mongomock.MongoClient)


    def test_send_email_message_success(self):
        """Test başarılı email mesaj gönderimi"""
        with patch('app.routes.email_routes.send_email_message') as mock_send:
            mock_send.return_value = True
            response = self.client.post('/api/email/create',
                                      json={
                                          'to_email': 'test@example.com',
                                          'subject': 'Test Subject',
                                          'message': 'Test Message'
                                      })
            
            data = json.loads(response.data)
            self.assertEqual(response.status_code, 200)
            self.assertTrue(data['isSuccess'])
            self.assertEqual(data['resultCode'], "200")


    def test_get_latest_email_message(self):
        """Test son email mesajını getirme"""
        mock_log = MagicMock()
        mock_log.to_mongo.return_value.to_dict.return_value = {
            '_id': 'test_id',
            'to_email': 'test@example.com',
            'type': 'message',
            'subject': 'Test Subject',
            'message': 'Test Message',
            'timestamp': '2025-05-04T10:00:00Z'
        }
        
        with patch('app.routes.email_routes.get_email_message_latest') as mock_get:
            mock_get.return_value = mock_log
            response = self.client.get('/api/email/get/test@example.com')
            
            data = json.loads(response.data)
            self.assertEqual(response.status_code, 200)
            self.assertTrue(data['isSuccess'])
            self.assertEqual(data['resultCode'], "200")


    def test_invalid_request_body(self):
        """Test geçersiz request body durumu"""
        response = self.client.post('/api/email/create', json={})
        data = json.loads(response.data)
        self.assertEqual(response.status_code, 400)
        self.assertFalse(data['isSuccess'])

if __name__ == '__main__':
    unittest.main()