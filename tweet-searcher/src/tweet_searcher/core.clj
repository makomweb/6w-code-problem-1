(ns tweet-searcher.core
  (:gen-class)
  (:require [clj-http.client :as http]
			[clojure.data.json :as json]))

(def consumer_key "")
(def consumer_secret "")
 		 
(defn request_twitter [search_term]
	(let [bearer_token
		(let [response_body
			(:body (http/post "https://api.twitter.com/oauth2/token"
						{:basic-auth [consumer_key consumer_secret]
						 :headers {:content-type "application/x-www-form-urlencoded;charset=UTF-8"}
						 :form-params {:grant_type "client_credentials"}}))]
			(:access_token (json/read-str response_body :key-fn keyword)))]			
		(:body (http/get "https://api.twitter.com/1.1/search/tweets.json"
					{:headers {:authorization (str "Bearer " bearer_token)}
					 :query-params {"q" search_term, "count" "100"}}))))
			 
(defn query_twitter [search_term]
	(let [response_body (request_twitter search_term)]
		(map :text (:statuses (json/read-str response_body :key-fn keyword)))))
	
(defn -main
  "I don't do a whole lot ... yet."
  [& args]
  (println "Hello, World!"))

  